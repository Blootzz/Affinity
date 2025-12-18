using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerHitbox playerHitbox;
    PlayerBaseState currentState;
    [SerializeField] private string currentStateName;
    [SerializeField] GameObject ledgeGrabChecker; // set in editor

    PlayerInput playerInput;
    [HideInInspector] public PlayerAnimationManager playerAnimationManager; // accessed by each state to play animations
    [HideInInspector] public CharacterMover characterMover;
    [HideInInspector] public CharacterJumper characterJumper;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [HideInInspector] public BlockParryManager blockParryManager; // accessed by PlayerParryingState
    [HideInInspector] public GroundCheck groundCheck; // accessed by Idle
    [HideInInspector] public WallCheck2 wallCheck;
    [HideInInspector] public Health playerHealth; // accessed by PlayerStateHurt
    [HideInInspector] public Poise playerPoise;
    [HideInInspector] public ColorFlash colorFlasher;
    [HideInInspector] public SHORYUKEN shoryukenChecker;

    bool flagHurtboxHit = false;
    bool flagBlockerHit = false;

    [Header("Basic Movement Settings")]
    public bool faceRight = true;
    [Tooltip("This gets fed to CharacterMover every time the PlayerStateRunning calls its HorizontalAxis method")]
    public float runSpeed = 6f;
    float lastSetXInput = 0; // used to track input when Idle state is called but a new Input Action hasn't fired yet
    float lastSetYInput = 0;
    bool lastSetBlockInput = false;
    bool lastInteractInput = false;

    [Header("State Logic Modifiers")]
    public bool isInvincible = false;

    [Header("State Data")]
    public Vector2 ledgeGrabPos;

    //[InspectorButton(nameof(OnButtonClicked))]
    //public bool EnableBlock;
    //private void OnButtonClicked() { DoStateBlock(true); }

    private void Awake()
    {
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        characterMover = GetComponent<CharacterMover>();
        characterJumper = GetComponent<CharacterJumper>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        wallCheck = GetComponent<WallCheck2>();
        hurtboxManager = GetComponentInChildren<HurtboxManager>();
        blockParryManager = GetComponentInChildren<BlockParryManager>();
        playerHealth = GetComponent<Health>();
        playerPoise = GetComponent<Poise>();
        colorFlasher = GetComponent<ColorFlash>();
        shoryukenChecker = GetComponent<SHORYUKEN>();
    }

    // Event Subscription
    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
        groundCheck.OnGroundedChanged += OnStateGroundedChange;
        wallCheck.OnWallCollisionChanged += OnWallCheckChange;
        hurtboxManager.HurtEvent += FlagOnPlayerHurtboxHit;
        blockParryManager.BlockerHitEvent += FlagOnBlockerHit;
        playerHealth.DeathEvent += OnDeath;
        playerPoise.PoiseDepletedEvent += OnPoiseDepleted;
        characterMover.HorVelocityHitZeroEvent += OnHorVelocityHitZero;
        ledgeGrabChecker.GetComponent<LedgeGrabChecker>().LedgeGrabEvent += OnLedgeGrabFound;
    }

    // Event Unsubscription
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
        groundCheck.OnGroundedChanged -= OnStateGroundedChange;
        wallCheck.OnWallCollisionChanged -= OnWallCheckChange;
        hurtboxManager.HurtEvent -= FlagOnPlayerHurtboxHit;
        blockParryManager.BlockerHitEvent -= FlagOnBlockerHit;
        playerHealth.DeathEvent -= OnDeath;
        playerPoise.PoiseDepletedEvent -= OnPoiseDepleted;
        characterMover.HorVelocityHitZeroEvent -= OnHorVelocityHitZero;
        ledgeGrabChecker.GetComponent<LedgeGrabChecker>().LedgeGrabEvent -= OnLedgeGrabFound;
    }

    private void Start()
    {
        currentState = new PlayerStateIdle(this);
    }

    // Used to process hurtbox and hitbox in order AFTER previous physics calculations have been done
    private void FixedUpdate()
    {
        EvaluateIncomingAttack();
    }

    void EvaluateIncomingAttack()
    {
        //if (flagBlockerHit || flagHurtboxHit)
        //    print("blocker: "+flagBlockerHit+" | hurtbox: "+flagHurtboxHit);

        if (flagBlockerHit)
        {
            OnBlockerHit();
            flagBlockerHit = false;

            // prevent player from getting hurt next loop
            flagHurtboxHit = false;
        }
        else if (flagHurtboxHit)
        {
            if (isInvincible)
                return;

            OnPlayerHurtboxHit();
            flagHurtboxHit = false;
        }
    }

    /// <summary>
    /// Switches manager's state
    /// </summary>
    /// <param name="newState">Pass in "new PlayerStateRunning(stateManager)"</param>
    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentStateName = newState.GetType().Name;
        currentState.OnEnter();
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        // ignore performed flag
        if (context.performed)
            return;

        if (context.action.name.Equals("HorizontalAxis"))
            DoStateHorizontal(context.ReadValue<float>());
        if (context.action.name.Equals("VerticalAxis"))
            DoStateVertical(context.ReadValue<float>());
        if (context.action.name.Equals("Jump"))
        {
            if (context.started)
                DoStateJump(true);
            if (context.canceled)
                DoStateJump(false);
        }
        
        if (context.action.name.Equals("Block"))
        {
            // either start blocking or release
            if (context.started)
                DoStateBlock(true);
            else if (context.canceled)
                DoStateBlock(false);
        }
        if (context.action.name.Equals("Attack"))
        {
            if (context.started)
                DoStateAttack();
        }

        if (context.action.name.Equals("Interact"))
        {
            if (context.started)
                DoStateInteract(true);
            else if (context.canceled)
                DoStateInteract(false);
        }
    }

    #region Horizontal Control
    void DoStateHorizontal(float xInput)
    {
        lastSetXInput = xInput;
        currentState.HorizontalAxis();
    }
    public float GetLastSetXInput()
    {
        return lastSetXInput;
    }
    #endregion

    #region Vertical Control
    void DoStateVertical(float yInput)
    {
        lastSetYInput = yInput;
        currentState.VerticalAxis();
    }
    public float GetLastSetYInput()
    {
        return lastSetYInput;
    }
    #endregion

    void DoStateJump(bool started)
    {
        if (started)
            currentState.JumpStart();
        else
            currentState.JumpCancel();
    }

    void DoStateBlock(bool started)
    {
        if (started)
        {
            lastSetBlockInput = true;
            currentState.BlockStart();
        }
        else
        {
            lastSetBlockInput = false;
            currentState.BlockCancel();
        }
    }
    public bool GetLastBlockInput()
    {
        return lastSetBlockInput;
    }

    void DoStateAttack()
    {
        if (shoryukenChecker.CheckShoryu())
            currentState.SHORYUKEN();
        else
            currentState.Attack();
    }

    void DoStateInteract(bool started)
    {
        if (started)
        {
            lastInteractInput = true;
            currentState.InteractStart();
        }
        else
        {
            lastInteractInput = false;
            currentState.InteractCancel();
        }
    }
    public void EnableLedgeGrabCheck(bool active)
    {
        ledgeGrabChecker.SetActive(active);
    }
    public bool GetLastInteractInput()
    {
        return lastInteractInput;
    }

    void OnLedgeGrabFound(Vector2 ledgePos)
    {
        ledgeGrabPos = ledgePos;
        SwitchState(new PlayerStateLedgeHang(this));
    }

    void OnStateGroundedChange(bool isGrounded)
    {
        currentState.ProcessGroundCheckEvent(isGrounded);
        if (isGrounded)
            characterJumper.ResetDoubleJump();
    }

    void OnWallCheckChange(bool isInWall)
    {
        currentState.ProcessWallCheckEvent(isInWall);
    }

    public void FlipIfNecessary()
    {
        if (lastSetXInput > 0 && !faceRight)
            Flip();
        else if (lastSetXInput < 0 && faceRight)
            Flip();
    }
    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(Vector3.up * 180);
    }

    void FlagOnPlayerHurtboxHit() => flagHurtboxHit = true;
    void OnPlayerHurtboxHit()
    {
        SwitchState(new PlayerStateHurt(this));
    }

    void FlagOnBlockerHit() => flagBlockerHit = true;
    void OnBlockerHit()
    {
        // temporarily disable hurtbox to prevent player getting hit on same frame
        hurtboxManager.DisableOneFrame();

        currentState.ProcessBlockerHit();
    }

    void OnDeath()
    {
        currentState.Die();
    }
    void OnPoiseDepleted()
    {
        if (currentStateName.Equals("PlayerStatePoiseDepleted"))
        {
            Debug.LogError("poise depleted again. fix this bug");
            return;
        }
        SwitchState(new PlayerStatePoiseDepleted(this));
    }

    void OnHorVelocityHitZero()
    {
        currentState.HorVelocityHitZero();
    }

    void OnShoryukenEvent()
    {
        currentState.SHORYUKEN();
    }

    // called by animations that end their state
    void ANIM_EndStateByAnimation()
    {
        currentState.EndStateByAnimation();
    }


    /// <summary>
    /// Blockers will be left open by previous block state
    /// </summary>
    public void ANIM_ParryWindowOpened()
    {
        blockParryManager.SetIsParryWindowOpen(true);
    }

    /// <summary>
    /// Necessary to disable blockers so that blockers don't save player from getting hit in PlayerStateManager.FixedUpdate()
    /// </summary>
    public void ANIM_ParryWindowClosed()
    {
        blockParryManager.SetEnableBlockers(false, false);
        blockParryManager.SetIsParryWindowOpen(false);
    }
}

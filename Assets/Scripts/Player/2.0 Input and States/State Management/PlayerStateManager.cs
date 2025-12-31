using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerHitbox playerHitbox;
    PlayerBaseState currentState;
    [SerializeField] private string currentStateName;
    [SerializeField] GameObject ledgeGrabChecker; // set in editor

    [Header("States-Map Basic")]
    public PlayerStateIdle playerStateIdle;
    public PlayerStateAttacking playerStateAttacking;
    public PlayerStateBlocking playerStateBlocking;
    public PlayerStateBlockSlide playerStateBlockSlide;
    public PlayerStateFalling playerStateFalling;
    public PlayerStateHurt playerStateHurt;
    public PlayerStateJumping playerStateJumping;
    public PlayerStateDoubleJumping playerStateDoubleJumping;
    public PlayerStateLedgeClimb playerStateLedgeClimb;
    public PlayerStateLedgeHang playerStateLedgeHang;
    public PlayerStateParrying playerStateParrying;
    public PlayerStatePoiseDepleted playerStatePoiseDepleted;
    public PlayerStateReaching playerStateReaching;
    public PlayerStateRunning playerStateRunning;
    public PlayerStateSHORYUKEN playerStateSHORYUKEN;
    public PlayerStateWallJumping playerStateWallJumping;
    public PlayerStateWallSlide playerStateWallSlide;
    [Header("States-Map Guitar")]
    public PlayerStateGuitar playerStateGuitar;

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
    bool lastLedgeGrabInput = false;

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
        wallCheck = GetComponentInChildren<WallCheck2>();
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
        characterJumper.HitApexEvent += OnFallingApexReached;
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
        characterJumper.HitApexEvent -= OnFallingApexReached;
    }

    private void Start()
    {
        SwitchState(playerStateIdle);
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
        currentState.SetStateManager(this);
        currentState.OnEnter();
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        // ignore performed flag
        if (context.performed)
            return;

        print("PlayerInput: " + playerInput.name);
        print("PlayerInput.currentActionMap: " + playerInput.currentActionMap.name);
        string currentMapName = playerInput.currentActionMap.name;

        if (currentMapName.Equals("Basic"))
            DoInputsBasicMap(context);
        else if (currentMapName.Equals("Guitar"))
            DoInputsGuitarMap(context);

    }
    void DoInputsBasicMap(InputAction.CallbackContext context)
    {
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

        if (context.action.name.Equals("LedgeGrab"))
        {
            if (context.started)
                DoStateLedgeGrab(true);
            else if (context.canceled)
                DoStateLedgeGrab(false);
        }

        if (context.action.name.Equals("Guitar"))
        {
            if (context.started)
                DoStateGuitar();
        }
    }
    void DoInputsGuitarMap(InputAction.CallbackContext context)
    {
        //if (context.action.name.Equals("1st"))
        //    guitarController.PlayNote(1);
        //if (context.action.name.Equals("2nd"))
        //    guitarController.PlayNote(2);
        //if (context.action.name.Equals("3rd"))
        //    guitarController.PlayNote(3);
        //if (context.action.name.Equals("4th"))
        //    guitarController.PlayNote(4);
        //if (context.action.name.Equals("5th"))
        //    guitarController.PlayNote(5);
        //if (context.action.name.Equals("6th"))
        //    guitarController.PlayNote(6);
        //if (context.action.name.Equals("7th"))
        //    guitarController.PlayNote(7);
        //if (context.action.name.Equals("8th"))
        //    guitarController.PlayNote(8);
        //if (context.action.name.Equals("9th"))
        //    guitarController.PlayNote(9);
        //if (context.action.name.Equals("10th"))
        //    guitarController.PlayNote(10);
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

    void DoStateLedgeGrab(bool started)
    {
        if (started)
        {
            lastLedgeGrabInput = true;
            currentState.LedgeGrabStarted();
        }
        else
        {
            lastLedgeGrabInput = false;
            currentState.LedgeGrabCanceled();
        }
    }
    public void EnableLedgeGrabCheck(bool active)
    {
        ledgeGrabChecker.SetActive(active);
    }
    public bool GetLastLedgeGrabInput()
    {
        return lastLedgeGrabInput;
    }

    void OnLedgeGrabFound(Vector2 ledgePos)
    {
        ledgeGrabPos = ledgePos;
        SwitchState(playerStateLedgeHang);
    }

    void DoStateGuitar()
    {
        currentState.OpenGuitar();
    }

    void OnStateGroundedChange(bool isGrounded)
    {
        currentState.ProcessGroundCheckEvent(isGrounded);
        if (isGrounded)
            characterJumper.ResetDoubleJump();
    }

    void OnWallCheckChange(bool isInWall)
    {
        if (isInWall)
            currentState.WallCheckEntered();
        else
            currentState.WallCheckExited();
    }

    void OnFallingApexReached()
    {
        currentState.FallingApexReached();
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
    /// <summary>
    /// Forcibly flips the player. Useful for Wall Jump
    /// </summary>
    public void ForceFlip()
    {
        Flip();
    }

    void FlagOnPlayerHurtboxHit() => flagHurtboxHit = true;
    void OnPlayerHurtboxHit()
    {
        SwitchState(playerStateHurt);
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
        SwitchState(playerStatePoiseDepleted);
    }

    void OnHorVelocityHitZero()
    {
        currentState.HorVelocityHitZero();
    }

    public void SwitchActionMap(string newMap)
    {
        print("Switching action map from " + playerInput.currentActionMap.name + " to " + newMap);
        playerInput.SwitchCurrentActionMap(/*newMap*/playerInput.actions.FindActionMap("Guitar").name);
        print("playerInput.currentActionMap.name: " + playerInput.currentActionMap.name);
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

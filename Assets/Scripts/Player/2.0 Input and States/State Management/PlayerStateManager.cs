using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [Header("Disabled Obj References")]
    public PlayerHitbox playerHitbox;
    [SerializeField] GameObject ledgeGrabChecker; // set in editor
    [SerializeField] RopeDetector ropeDetector;
    public GuitarController guitarController;

    [Header("Explicit Obj References")]
    public WallCheck2 wallJumpCheck;
    public WallCheck2 fallingMovementWallCheck;

    [Header("States-Basic Action Map")]
    public PlayerStateIdle playerStateIdle;
    public PlayerStateAttacking playerStateAttacking;
    public PlayerStateBlocking playerStateBlocking;
    public PlayerStateBlockSlide playerStateBlockSlide;
    public PlayerStateCrouching playerStateCrouching;
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
    public PlayerStateZiplineForward playerStateZiplineForward;
    [Header("States-Guitar Action Map")]
    public PlayerStateGuitar playerStateGuitar;

    PlayerInput playerInput;
    [HideInInspector] public PlayerAnimationManager playerAnimationManager; // accessed by each state to play animations
    [HideInInspector] public CharacterMover characterMover;
    [HideInInspector] public CharacterJumper characterJumper;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [HideInInspector] public BlockParryManager blockParryManager; // accessed by PlayerParryingState
    [HideInInspector] public GroundCheck groundCheck; // accessed by Idle
    [HideInInspector] public Health playerHealth; // accessed by PlayerStateHurt
    [HideInInspector] public Poise playerPoise;
    [HideInInspector] public ColorFlash colorFlasher;
    [HideInInspector] public SHORYUKEN shoryukenChecker;
    [HideInInspector] public Rope ropeController;

    [Header("Current State")]
    [SerializeField] private string currentStateName;
    PlayerBaseState currentState;

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
        wallJumpCheck.OnWallCollisionChanged += OnwallJumpCheckChange;
        fallingMovementWallCheck.OnWallCollisionChanged += OnFallingWallCheckChange;
        hurtboxManager.HurtEvent += FlagOnPlayerHurtboxHit;
        blockParryManager.BlockerHitEvent += FlagOnBlockerHit;
        playerHealth.DeathEvent += OnDeath;
        playerPoise.PoiseDepletedEvent += OnPoiseDepleted;
        characterMover.HorVelocityHitZeroEvent += OnHorVelocityHitZero;
        ledgeGrabChecker.GetComponent<LedgeGrabChecker>().LedgeGrabEvent += OnLedgeGrabFound;
        characterJumper.HitApexEvent += OnFallingApexReached;
        ropeDetector.RopeFoundEvent += OnRopeFound;
    }

    // Event Unsubscription
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
        groundCheck.OnGroundedChanged -= OnStateGroundedChange;
        wallJumpCheck.OnWallCollisionChanged -= OnwallJumpCheckChange;
        fallingMovementWallCheck.OnWallCollisionChanged -= OnFallingWallCheckChange;
        hurtboxManager.HurtEvent -= FlagOnPlayerHurtboxHit;
        blockParryManager.BlockerHitEvent -= FlagOnBlockerHit;
        playerHealth.DeathEvent -= OnDeath;
        playerPoise.PoiseDepletedEvent -= OnPoiseDepleted;
        characterMover.HorVelocityHitZeroEvent -= OnHorVelocityHitZero;
        ledgeGrabChecker.GetComponent<LedgeGrabChecker>().LedgeGrabEvent -= OnLedgeGrabFound;
        characterJumper.HitApexEvent -= OnFallingApexReached;
        ropeDetector.RopeFoundEvent -= OnRopeFound;
    }

    private void Start()
    {
        SwitchState(playerStateIdle);
    }

    // Used to process hurtbox and hitbox in order AFTER previous physics calculations have been done
    private void FixedUpdate()
    {
        EvaluateIncomingAttack();
        DoStateFixedUpdate();
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
        //if (newState != null && currentState != null)
        //    print("Switching from " + currentState.name + " to " + newState.name);
        currentState = newState;
        currentStateName = newState.GetType().Name;
        currentState.SetStateManager(this);
        currentState.OnEnter();
    }

    // ================================================ Input ============================================
    #region Input
    public void SwitchActionMap(string newMap)
    {
        playerInput.SwitchCurrentActionMap(newMap);
    }

    /// NEVER USE playerInput.currentActionMap BECAUSE IT MAY NOT GET UPDATED IN SEQUENCE WITH MAP CHANGES.
    /// use context.action.actionMap instead
    /// this is because any action that hasn't been canced gets cancelled and called with its old action map reference
    void OnActionTriggered(InputAction.CallbackContext context)
    {
        // ignore performed flag
        if (context.performed)
            return;

        string currentMapName = context.action.actionMap.name;

        if (currentMapName.Equals("Basic"))
            DoInputsBasicMap(context);
        else if (currentMapName.Equals("Guitar"))
            DoInputsGuitarMap(context);
    }
    void DoInputsBasicMap(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("HorizontalAxis"))
        {
            DoStateHorizontal(context.ReadValue<float>());
            return;
        }
        if (context.action.name.Equals("VerticalAxis"))
        {
            DoStateVertical(context.ReadValue<float>());
            return;
        }
        if (context.action.name.Equals("Jump"))
        {
            if (context.started)
                DoStateJump(true);
            if (context.canceled)
                DoStateJump(false);
            return;
        }
        
        if (context.action.name.Equals("Block"))
        {
            // either start blocking or release
            if (context.started)
                DoStateBlock(true);
            else if (context.canceled)
                DoStateBlock(false);
            return;
        }
        if (context.action.name.Equals("Attack"))
        {
            if (context.started)
                DoStateAttack();
            return;
        }
        if (context.action.name.Equals("LedgeGrab"))
        {
            if (context.started)
                DoStateLedgeGrab(true);
            else if (context.canceled)
                DoStateLedgeGrab(false);
            return;
        }
        if (context.action.name.Equals("Guitar"))
        {
            if (context.started)
                DoStateGuitar();
            return;
        }
        if (context.action.name.Equals("Exit"))
        {
            if (context.started)
                DoStateExit();
            return;
        }
    }
    void DoInputsGuitarMap(InputAction.CallbackContext context)
    {
        // actions that can be either started or canceled
        if (context.action.name.Equals("MajorChord"))
        {
            ((PlayerStateGuitar)currentState).ApplyChord(ChordType.MajorChord, context.started);
            return;
        }
        if (context.action.name.Equals("MinorChord"))
        {
            ((PlayerStateGuitar)currentState).ApplyChord(ChordType.MinorChord, context.started);
            return;
        }
        if (context.action.name.Equals("PowerChord"))
        {
            ((PlayerStateGuitar)currentState).ApplyChord(ChordType.PowerChord, context.started);
            return;
        }

        if (context.action.name.Equals("Sustain"))
        {
            ((PlayerStateGuitar)currentState).UseSustain(context.started);
            return;
        }

        if (context.action.name.Equals("CycleScaleForward"))
        {
            ((PlayerStateGuitar)currentState).CycleScale(true, context.started);
            return;
        }
        if (context.action.name.Equals("CycleScaleBackward"))
        {
            ((PlayerStateGuitar)currentState).CycleScale(false, context.started);
            return;
        }
        if (context.action.name.Equals("CycleKeyRootForward"))
        {
            ((PlayerStateGuitar)currentState).CycleKey(true, context.started);
            return;
        }
        if (context.action.name.Equals("CycleKeyRootBackward"))
        {
            ((PlayerStateGuitar)currentState).CycleKey(false, context.started);
            return;
        }
        if (context.action.name.Equals("BendHalf"))
        {
            ((PlayerStateGuitar)currentState).Bend(true, context.started);
            return;
        }
        if (context.action.name.Equals("BendWhole"))
        {
            ((PlayerStateGuitar)currentState).Bend(false, context.started);
            return;
        }

        if (context.action.name.Equals("HideMenu"))
        {
            ((PlayerStateGuitar)currentState).ToggleHideMenu(context.started);
        }

        if (context.action.name.Equals("1st"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(1, context.started);
            return;
        }
        if (context.action.name.Equals("2nd"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(2, context.started);
            return;
        }
        if (context.action.name.Equals("3rd"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(3, context.started);
            return;
        }
        if (context.action.name.Equals("4th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(4, context.started);
            return;
        }
        if (context.action.name.Equals("5th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(5, context.started);
            return;
        }
        if (context.action.name.Equals("6th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(6, context.started);
            return;
        }
        if (context.action.name.Equals("7th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(7, context.started);
            return;
        }
        if (context.action.name.Equals("8th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(8, context.started);
            return;
        }
        if (context.action.name.Equals("9th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(9, context.started);
            return;
        }
        if (context.action.name.Equals("10th"))
        {
            ((PlayerStateGuitar)currentState).PlayNote(10, context.started);
            return;
        }
       
        // ======================== don't accept canceled note inputs ============================
        if (context.canceled)
            return;

        if (context.action.name.Equals("IncrementGuitarSpriteUp"))
        {
            ((PlayerStateGuitar)currentState).IncrementGuitarSprite(true);
            return;
        }
        if (context.action.name.Equals("IncrementGuitarSpriteDown"))
        {
            ((PlayerStateGuitar)currentState).IncrementGuitarSprite(false);
            return;
        }
        if (context.action.name.Equals("Exit"))
        {
            DoStateExit();
            return;
        }
    }
    #endregion

    // ============================== Getters =============================================
    #region Getters
    public float GetLastSetXInput()
    {
        return lastSetXInput;
    }
    public float GetLastSetYInput()
    {
        return lastSetYInput;
    }
    public bool GetLastBlockInput()
    {
        return lastSetBlockInput;
    }
    public bool GetLastLedgeGrabInput()
    {
        return lastLedgeGrabInput;
    }
    #endregion

    // ============================= DoState Methods ===============================
    #region DoState Methods
    void DoStateHorizontal(float xInput)
    {
        lastSetXInput = xInput;
        currentState.HorizontalAxis();
    }
    void DoStateVertical(float yInput)
    {
        lastSetYInput = yInput;
        currentState.VerticalAxis();
    }
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

    void DoStateGuitar()
    {
        currentState.OpenGuitar();
    }

    void DoStateExit()
    {
        currentState.Exit();
    }

    void DoStateFixedUpdate()
    {
        currentState.DoFixedUpdate();
    }
    #endregion

    //========================================== Event Listeners ===================================
    #region Event Listeners
    void OnStateGroundedChange(bool isGrounded)
    {
        currentState.ProcessGroundCheckEvent(isGrounded);
        if (isGrounded)
            characterJumper.ResetDoubleJump();
    }

    /// <summary>
    /// used for wall jump
    /// </summary>
    void OnwallJumpCheckChange(bool isInWall)
    {
        if (isInWall)
            currentState.WallJumpCheckEntered();
        else
            currentState.WallJumpCheckExited();
    }
    /// <summary>
    /// Used for allowing player to continue moving after a wall has been jumped over
    /// </summary>
    void OnFallingWallCheckChange(bool isInFallingWall)
    {
        currentState.FallingWallCheckChanged(isInFallingWall);
    }

    void OnFallingApexReached()
    {
        currentState.FallingApexReached();
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

    void OnLedgeGrabFound(Vector2 ledgePos)
    {
        ledgeGrabPos = ledgePos;
        SwitchState(playerStateLedgeHang);
    }
    void OnRopeFound(Rope ropeController)
    {
        playerStateZiplineForward.ropeController = ropeController;
        SwitchState(playerStateZiplineForward);
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

    #endregion

    // ================================ Executive Methods ================================
    #region Executive Methods
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

    public void EnableLedgeGrabCheck(bool active)
    {
        ledgeGrabChecker.SetActive(active);
    }
    #endregion


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

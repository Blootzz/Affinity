using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerBaseState currentState;
    [SerializeField] private string currentStateName;
    [HideInInspector] public PlayerAnimationManager playerAnimationManager; // accessed by each state to play animations
    [HideInInspector] public CharacterMover characterMover;
    [HideInInspector] public CharacterJumper characterJumper;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [HideInInspector] public BlockParryManager blockParryManager; // accessed by PlayerParryingState
    [HideInInspector] public GroundCheck groundCheck; // accessed by Idle
    [HideInInspector] public Health playerHealth; // accessed by PlayerStateHurt
    [HideInInspector] public Poise playerPoise;
    public PlayerHitbox playerHitbox;

    [Header("Basic Movement Settings")]
    public bool faceRight = true;
    [Tooltip("This gets fed to CharacterMover every time the PlayerStateRunning calls its HorizontalAxis method")]
    public float runSpeed = 6f;
    float lastSetXInput = 0; // used to track input when Idle state is called but a new Input Action hasn't fired yet
    float lastSetYInput = 0;
    bool lastSetBlockInput = false;

    [InspectorButton(nameof(OnButtonClicked))]
    public bool EnableBlock;
    private void OnButtonClicked() { DoStateBlock(true); }
    
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
    }

    // Event Subscription
    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
        groundCheck.OnGroundedChanged += OnStateGroundedChange;
        hurtboxManager.HurtEvent += OnPlayerHurboxHit;
        blockParryManager.BlockerHitEvent += OnBlockerHit;
        playerHealth.DeathEvent += OnDeath;
        playerPoise.PoiseDepletedEvent += OnPoiseDepleted;
    }

    // Event Unsubscription
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
        groundCheck.OnGroundedChanged -= OnStateGroundedChange;
        hurtboxManager.HurtEvent -= OnPlayerHurboxHit;
        blockParryManager.BlockerHitEvent -= OnBlockerHit;
        playerHealth.DeathEvent -= OnDeath;
        playerPoise.PoiseDepletedEvent -= OnPoiseDepleted;
    }

    private void Start()
    {
        currentState = new PlayerStateIdle(this);
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
        currentState.Attack();
    }

    void OnStateGroundedChange(bool isGrounded)
    {
        currentState.ProcessGroundCheckEvent(isGrounded);
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

    void OnPlayerHurboxHit()
    {
        SwitchState(new PlayerStateHurt(this));
    }
    void OnBlockerHit()
    {
        currentState.ProcessBlockerHit();
    }

    void OnDeath()
    {
        currentState.Die();
    }
    void OnPoiseDepleted()
    {
        currentState.PoiseDepleted();
    }

    // called by animations that end their state
    void ANIM_EndStateByAnimation()
    {
        currentState.EndStateByAnimation();
    }
}

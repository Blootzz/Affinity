using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerBaseState currentState;
    [SerializeField] private string currentStateName;
    [HideInInspector] public PlayerAnimationManager playerAnimationManager;
    [HideInInspector] public CharacterMover characterMover;
    [HideInInspector] public CharacterJumper characterJumper;
    GroundCheck groundCheck;

    [Header("Basic Movement Settings")]
    public bool faceRight = true;
    [Tooltip("This gets fed to CharacterMover every time the PlayerStateRunning calls its HorizontalAxis method")]
    public float runSpeed = 6f;
    float lastSetXInput = 0; // used to track input when Idle state is called but a new Input Action hasn't fired yet

    private void Awake()
    {
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        characterMover = GetComponent<CharacterMover>();
        characterJumper = GetComponent<CharacterJumper>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    // Event Subscription
    private void OnEnable()
    {
        playerInput.onActionTriggered += OnActionTriggered;
        groundCheck.OnGroundedChanged += DoStateGroundedChange;
    }
    // Event Unsubscription
    private void OnDisable()
    {
        playerInput.onActionTriggered -= OnActionTriggered;
        groundCheck.OnGroundedChanged -= DoStateGroundedChange;
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
        if (context.action.name.Equals("Jump"))
        {
            if (context.started)
                DoStateJumpStart();
            if (context.canceled)
                DoStateJumpCancel();
        }
        
        if (context.action.name.Equals("Block"))
        {
            // either start blocking or release
            if (context.started)
                DoStateBlock(true);
            else if (context.canceled)
                DoStateBlock(false);
        }
        if (context.action.name.Equals("Parry"))
            DoStateParry();
    }

    void DoStateHorizontal(float xInput)
    {
        lastSetXInput = xInput;
        currentState.HorizontalAxis();
    }
    public float GetLastSetXInput()
    {
        return lastSetXInput;
    }

    // default behaviour is Jump, can be overridden
    void DoStateJumpStart()
    {
        currentState.JumpStart();
    }
    void DoStateJumpCancel()
    {
        currentState.JumpCancel();
    }

    void DoStateBlock(bool newState)
    {

    }

    void DoStateParry()
    {

    }

    void DoStateGroundedChange(bool isGrounded)
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

}

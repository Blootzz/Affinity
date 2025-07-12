using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerBaseState currentState;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterMover characterMover;

    [Header("Basic Movement Settings")]
    [Tooltip("This gets fed to CharacterMover every time the PlayerStateRunning calls its HorizontalAxis method")]
    public float runSpeed = 6f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterMover = GetComponent<CharacterMover>();
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }

    private void Start()
    {
        currentState = new PlayerStateIdle(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("HorizontalAxis"))
            DoStateHorizontal(context.ReadValue<float>());
        if (context.action.name.Equals("Jump"))
            DoStateJump();
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
        currentState.HorizontalAxis(xInput);
    }

    // default behaviour is Jump, can be overridden
    void DoStateJump()
    {

    }

    void DoStateBlock(bool newState)
    {

    }

    void DoStateParry()
    {

    }
}

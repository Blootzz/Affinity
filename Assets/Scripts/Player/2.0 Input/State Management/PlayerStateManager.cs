using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerBaseState currentState;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterMover characterMover;

    [Header("Basic Movement Settings")]
    public float runSpeed = 8f;

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
        if (context.action.name.Equals("Move"))
            DoStateWASD(context.ReadValue<Vector2>());
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

    void DoStateWASD(Vector2 xyInput)
    {
        currentState.WASD(xyInput);
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

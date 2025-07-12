using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();
        playerInput.onActionTriggered += OnActionTriggered;
    }

    void OnActionTriggered(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("Move"))
            Move(context.ReadValue<float>());
        if (context.action.name.Equals("Jump"))
            Jump();
        if (context.action.name.Equals("Block"))
        {
            // either start blocking or release
            if (context.started)
                Block(true);
            else if (context.canceled)
                Block(false);
        }
        if (context.action.name.Equals("Parry"))
            Parry();
    }

    void Move(float val)
    {

    }

    void Jump()
    {

    }

    void Block(bool newState)
    {

    }

    void Parry()
    {

    }
}

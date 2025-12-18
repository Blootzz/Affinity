using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SHORYUKEN : MonoBehaviour
{
    [SerializeField]
    float commandInputTiming = 0.15f; // how long player can wait to add to command input

    PlayerInput playerInput;
    public event Action EventTriggerSHORYUKEN; // listened to in PlayerStateManager

    char[] inputTracker = new char[4]; // records inputs as N,E,S,W regardless of if they are the right inputs
    int targetIndex = 0; // determines what index of the array should be filled

    Rigidbody2D rb;
    int Shoryuken; // animation

    // Start is called before the first frame update
    void Start()
    {
        // initially filled with Z chars
        ClearInputs();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        playerInput.onActionTriggered += ShoryuOnActionTriggered;
    }
    private void OnDisable()
    {
        playerInput.onActionTriggered -= ShoryuOnActionTriggered;
    }

    /// <summary>
    /// Listens to cardinal directions.
    /// Listening to attack event is ONLY done in state manager to avoid event sequence ambiguity
    /// </summary>
    /// <param name="context"></param>
    void ShoryuOnActionTriggered(InputAction.CallbackContext context)
    {
        // only process positive inputs
        if (!context.started)
            return;

        if (context.action.name.Equals("HorizontalAxis"))
            ProcessHorizontalInput(context.ReadValue<float>());
        if (context.action.name.Equals("VerticalAxis"))
            ProcessVerticalInput(context.ReadValue<float>());
    }

    /// <summary>
    /// Uses <code>FillArray() to pass in cardinal direction char</code>
    /// </summary>
    /// <param name="input"></param>
    void ProcessHorizontalInput(float input)
    {
        if (input > 0)
            FillArray('E'); // east
        else
            FillArray('W'); // west
    }
    void ProcessVerticalInput(float input)
    {
        if (input > 0)
            FillArray('N'); // north
        else
            FillArray('S');
    }

    void ProcessDirectionalInput(float xInput, float yInput)
    {
        
    }

    void FillArray(char inputDirection) // adds one char to the array
    {
        // prevent duplicate direction inputs to allow controllers to work

        CancelInvoke();
        Invoke(nameof(ClearInputs), commandInputTiming);

        // fill 0-4 position with input
        inputTracker[targetIndex] = inputDirection;

        if (targetIndex == inputTracker.Length - 1) // last position was just filled
        {
            // move everything in array back, overwriting the data in inputTracker[0]
            for (int i = 0; i < inputTracker.Length - 1; i++) // stops at second to last index
            {
                inputTracker[i] = inputTracker[i + 1];
            }// this leaves the last index unchanged, but that value will never be compared

            // targetIndex is not incremented and never will be as it has reached its max value
        }
        else
        {// targetIndex is either 0, 1, or 2.
            targetIndex++;
        }
    }

    public bool CheckShoryu()
    {
        if (inputTracker[0] == 'E' && inputTracker[1] == 'S' && inputTracker[2] == 'E')
            return true;
        if (inputTracker[0] == 'W' && inputTracker[1] == 'S' && inputTracker[2] == 'W')
            return true;
        return false;
    }

    /// <summary>
    /// Call this when the Shoryuken state and animation actually go off
    /// </summary>
    public void OnExecutionClearInputs()
    {
        ClearInputs();
    }

    void ClearInputs()
    {
        targetIndex = 0;
        for (int i = 0; i < inputTracker.Length; i++)
        {
            inputTracker[i] = 'Z';
        }
    }

}

public class PlayerStateSHORYUKEN : PlayerStateAttacking
{
    [SerializeField] float shoryuDamage = 30;
    [SerializeField] Vector2 shoryuSpeed = new Vector2(2, 12);

    public PlayerStateSHORYUKEN(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        stateManager.playerHitbox.SetDamage(shoryuDamage);

        // animation sets playerHitbox.SetActive to true
        stateManager.playerAnimationManager.PlayAnimation(stateManager.playerAnimationManager.AorUSHORYUKEN);
        stateManager.shoryukenChecker.OnExecutionClearInputs();
        DoShoryu();
    }

    void DoShoryu()
    {
        stateManager.characterMover.SetVelocity(shoryuSpeed);
    }

    public override void EndStateByAnimation()
    {
        stateManager.SwitchState(new PlayerStateFalling(stateManager));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SHORYUKEN : MonoBehaviour
{
    [Tooltip("how long player can wait to add to command input")]
    [SerializeField] float commandInputTiming = 0.15f;

    PlayerInput playerInput;
    public event Action EventTriggerSHORYUKEN; // listened to in PlayerStateManager

    char[] answerKeyRight = { '6', '2', '3' };
    char[] answerKeyLeft = { '4', '2', '1' };
    char[] inputTracker = new char[4]; // records inputs as N,E,S,W regardless of if they are the right inputs
    int targetIndex = 0; // determines what index of the array should be filled
    float xInput = 0;
    float yInput = 0;

    Rigidbody2D rb;
    int Shoryuken; // animation

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        // initially filled with Z chars
        ClearInputs();
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
    void ShoryuOnActionTriggered(InputAction.CallbackContext context)
    {
        // must take in started and cancelled input so it can zero out
        if (!context.started && !context.canceled)
            return;

        // only process horizontal or vertical inputs, storying them into inputX and inputY
        if (context.action.name.Equals("HorizontalAxis"))
        {
            //ProcessHorizontalInput(context.ReadValue<float>());
            xInput = context.ReadValue<float>();
        }
        else if (context.action.name.Equals("VerticalAxis"))
        {
            yInput = context.ReadValue<float>();
            //ProcessVerticalInput(context.ReadValue<float>());
        }
        else
            return;

        ProcessDirectionalInput();
    }

    /// <summary>
    /// Uses <code>FillArray()</code>to pass in cardinal direction char
    /// </summary>
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

    /// <summary>
    /// uses fighting game annotation for char of numbered positions (same as number pad on keyboard)
    /// 
    /// 789
    /// 456
    /// 123
    /// 
    /// </summary>
    void ProcessDirectionalInput()
    {
        // don't do anything if neutral
        if (xInput == 0 && yInput == 0)
            return;

        // angle is 0 on right, 90 top, +/- 180 left, -90 bottom
        float angle = Mathf.Atan2(yInput, xInput) * Mathf.Rad2Deg;
        //print("angle: " + angle);

        // section circle into 8ths (45 degrees), offest by 22.5 degrees
        // -157.5, -112.5, -67.5, -22.5, 22.5, 67.5, 112.5, 157.5 through -157.5
        // CCW border of input zone will be inclusive
        if (angle <= -157.5 || angle > 157.5)
        {
            FillArray('4'); // west direction
            return;
        }
        if (angle <= -112.5)
        {
            FillArray('1'); // southwest direction
            return;
        }
        if (angle <= -67.5)
        {
            FillArray('2'); // South
            return;
        }
        if (angle <= -22.5)
        {
            FillArray('3'); // southeast
            return;
        }
        if (angle <= 22.5)
        {
            FillArray('6'); // east
            return;
        }
        if (angle <= 67.5)
        {
            FillArray('9'); // northeast
            return;
        }
        if (angle <= 112.5)
        {
            FillArray('8'); // North
            return;
        }
        if (angle <= 157.5)
        {
            FillArray('7'); // northwest
            return;
        }
    }

    void FillArray(char inputDirection) // adds one char to the array
    {
        // prevent duplicate direction inputs to allow controllers to work. An analog stick may pass in a new value every frame if it's moving
        if (targetIndex > 0 && inputDirection == inputTracker[targetIndex-1])
            return;

        CancelInvoke();
        Invoke(nameof(ClearInputs), /*commandInputTiming*/3);

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
        
        //print("inputTracker");
        //foreach (char entry in inputTracker)
        //    print(entry + " ");


        print("Check Shoryu: "+CheckShoryu());

    }

    // must be able to forgive intermediate misinputs to make inputs feasible on stick
    public bool CheckShoryu()
    {
        //if (inputTracker[0] == '6' && inputTracker[1] == '2' && inputTracker[2] == '3') // rightward
        //    return true;
        //if (inputTracker[0] == '4' && inputTracker[1] == '2' && inputTracker[2] == '1') // leftward
        //    return true;
        //return false;

        // directions must be separated so that it doesn't allow mixing and matching answer keys to interpret command input

        // left facing
        int numMatchesRequired = answerKeyLeft.Length;
        int numMatches = 0;
        int iAnswerProgress = -1; // index to start with when parsing answerKey. Start here on next inputTracker (outer) loop. Updates when a match is found
        for (int iInput = 0; iInput < inputTracker.Length; iInput++)
        {
            for (int iKey = iAnswerProgress + 1; iKey < answerKeyLeft.Length; iKey++)
            {
                if (inputTracker[iInput] == answerKeyLeft[iKey])
                {
                    numMatches++;
                    if (numMatches == numMatchesRequired)
                    {
                        return true;
                    }
                    else // found a match, but not done
                    {
                        iAnswerProgress = iKey;
                        break;
                    }
                }
            }

        }

        // right facing
        numMatchesRequired = answerKeyRight.Length;
        numMatches = 0;
        iAnswerProgress = -1; // index to start with when parsing answerKey. Start here on next inputTracker (outer) loop. Updates when a match is found
        for (int iInput = 0; iInput < inputTracker.Length; iInput++)
        {
            for (int iKey = iAnswerProgress + 1; iKey < answerKeyRight.Length; iKey++)
            {
                if (inputTracker[iInput] == answerKeyRight[iKey])
                {
                    numMatches++;
                    if (numMatches == numMatchesRequired)
                        return true;
                    else // found a match, but not done
                    {
                        iAnswerProgress = iKey;
                        break;
                    }
                }
            }

        }

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
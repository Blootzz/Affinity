using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHORYUKEN : MonoBehaviour
{
    char[] inputTracker = new char[4]; // records inputs as N,E,S,W regardless of if they are the right inputs
    int targetIndex = 0; // determines what index of the array should be filled
    [SerializeField]
    float commandInputTiming = 0.15f; // how long player can wait to add to command input
    [SerializeField]
    Vector2 shoryuSpeed = new Vector2(2, 12);
    float shoryuDamage = 25f;
    float shoryuKnockbackMultiplier = 5f;
    Vector2 shoryuKnockbackAngle = new Vector2(1, 3);

    Rigidbody2D rb;
    Animator animator;
    int Shoryuken; // animation

    // Start is called before the first frame update
    void Start()
    {
        // initially filled with Z chars
        ClearInputs();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        Shoryuken = Animator.StringToHash("SHORYUKEN");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GameMaster.GM.controlManager.upKey))
        {
            FillArray('N');
        }
        if (Input.GetKeyDown(GameMaster.GM.controlManager.rightKey))
        {
            FillArray('E');
        }
        if (Input.GetKeyDown(GameMaster.GM.controlManager.downKey))
        {
            FillArray('S');
        }
        if (Input.GetKeyDown(GameMaster.GM.controlManager.leftKey))
        {
            FillArray('W');
        }
    }

    void FillArray(char inputDirection) // adds one char to the array
    {
        CancelInvoke();
        Invoke(nameof(ClearInputs), commandInputTiming);

        // fill 0-4 position with input
        inputTracker[targetIndex] = inputDirection;
        
        if (targetIndex == inputTracker.Length-1) // last position was just filled
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

    public void DoShoryu()
    {
        Player thePlayer = GetComponent<Player>();
        ClearInputs();

        thePlayer.attacking = true;
        rb.isKinematic = true;
        rb.linearVelocity = new Vector2(thePlayer.faceRight ? shoryuSpeed.x : -shoryuSpeed.x, shoryuSpeed.y);

        thePlayer.SetUpAttack(shoryuDamage, 0.2f, shoryuKnockbackMultiplier, shoryuKnockbackAngle);
        animator.Play(Shoryuken);
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

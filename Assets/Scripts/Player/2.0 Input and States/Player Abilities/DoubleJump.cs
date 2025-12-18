using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleJump : MonoBehaviour
{
    [SerializeField]
    private bool canDoubleJump = true;
    [SerializeField]
    private float doubleJumpHeight = 5f;

    public bool TryDoubleJump()
    {
        if (canDoubleJump)
        {
            print("Execute jump in playerstatemanager");
            canDoubleJump = false;
            return true;
        }
        return false;
    }

    public void ResetDoubleJump()
    {
        canDoubleJump = true;
    }
}

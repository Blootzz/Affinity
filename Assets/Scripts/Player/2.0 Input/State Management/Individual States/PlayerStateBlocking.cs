using UnityEngine;

public class PlayerStateBlocking : PlayerBaseState
{
    public PlayerStateBlocking(PlayerStateManager newStateManager) : base(newStateManager)
    {

    }

    public override void OnEnter()
    {
        animator.SetBool("isBlocking", true);
    }

    public override void OnExit()
    {
        animator.SetBool("isBlocking", false);
    }

    public override void HorizontalAxis(float xInput)
    {
        Debug.Log("implement directional block here");
    }
}

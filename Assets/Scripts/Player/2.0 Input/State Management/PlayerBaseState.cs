using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateManager stateManager;

    // Constructor sets stateManager
    public PlayerBaseState(PlayerStateManager newStateManager)
    {
        this.stateManager = newStateManager;
    }

    public virtual void OnEnter()
    {
    }
    public virtual void OnExit()
    {
    }
    
    public virtual void HorizontalAxis(float xInput)
    {
    }

    public virtual void Jump()
    {
    }
}

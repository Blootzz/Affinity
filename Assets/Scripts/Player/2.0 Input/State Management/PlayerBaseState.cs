using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerStateManager stateManager;
    protected Animator animator => stateManager.animator; // sets animator reference so that inheriting states can have access

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
}

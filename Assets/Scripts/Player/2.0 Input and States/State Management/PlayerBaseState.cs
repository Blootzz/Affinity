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
    
    public virtual void HorizontalAxis()
    {
    }
    public virtual void VerticalAxis()
    {
    }

    public virtual void JumpStart()
    {
    }
    public virtual void JumpCancel()
    {
    }

    public virtual void ProcessGroundCheckEvent(bool isGrounded)
    {
    }

    public virtual void BlockStart()
    {
    }
    public virtual void BlockCancel()
    {
    }

    public virtual void Attack()
    {
    }
    public virtual void ProcessBlockerHit()
    {
    }

    public virtual void EndStateByAnimation()
    {
    }

    public virtual void Die()
    {
        Debug.Log("Implement Death Here");
    }
    public virtual void PoiseDepleted()
    {
        Debug.Log("Implement PoiseDepleted Here");
    }
}

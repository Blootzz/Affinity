using UnityEngine;

public abstract class PlayerBaseState : ScriptableObject
{
    protected PlayerStateManager stateManager;

    //// Constructor sets stateManager
    //public PlayerBaseState(PlayerStateManager newStateManager)
    //{
    //    this.stateManager = newStateManager;
    //}
    public void SetStateManager(PlayerStateManager runningStateManager)
    {
        stateManager = runningStateManager;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    
    public virtual void HorizontalAxis() { }
    public virtual void VerticalAxis() { }

    public virtual void JumpStart() { }
    public virtual void JumpCancel() { }

    public virtual void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded)
            stateManager.SwitchState(stateManager.playerStateIdle);
        else
            stateManager.SwitchState(stateManager.playerStateFalling);
    }

    public virtual void BlockStart() { }
    public virtual void BlockCancel() { }

    public virtual void Attack() { }
    public virtual void ProcessBlockerHit() { }

    public virtual void EndStateByAnimation() { }

    public virtual void Die()
    {
        Debug.Log("Implement Death Here");
    }

    public virtual void HorVelocityHitZero() { }

    public virtual void LedgeGrabStarted() { }
    public virtual void LedgeGrabCanceled() { }
    public virtual void SHORYUKEN() { }
    
    public virtual void WallCheckEntered() { }
    public virtual void WallCheckExited() { }
    public virtual void FallingApexReached() { }
    public virtual void OpenGuitar() { }
    public virtual void PlayNote(int noteNum) { }
    public virtual void ApplyChord(ChordType chordNum) { }
    public virtual void UseSustain(bool useSustain) { }
    public virtual void IncrementGuitarSprite(bool forward) { }
}

using UnityEngine;
using System;

public abstract class EnemyBaseState : ScriptableObject
{
    protected EnemyStateManager stateManager;
    [SerializeField] protected AnimationClip stateAnimation;

    public void SetStateManager(EnemyStateManager newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public virtual void OnEnter() {
        if (stateAnimation != null)
            stateManager.animator.Play(stateAnimation.name);
        else
            Debug.LogWarning("No animation assigned for " + stateManager.gameObject.name + " state " + stateManager.currentStateName);
    }
    public virtual void OnExit() { }
    public virtual void EndStateByAnimation() { }
    public virtual void OnPlayerEnteredAttackZone() { }
    public virtual void OnPlayerExitedAttackZone()
    { 
        stateManager.SwitchState(stateManager.stateIdle); 
    }

    // chatgpt code lmao
    protected bool AnimatorHasClip(Animator animator, string clipName)
    {
        RuntimeAnimatorController rac = animator.runtimeAnimatorController;
        foreach (var clip in rac.animationClips)
        {
            if (clip.name == clipName)
                return true;
        }
        return false;
    }

    public virtual void BeginLerpToPlayerByAnimation()
    { // implement on individual attack so that strength can be custom per each attack
    }
    public virtual void EndLerpToPlayerByAnimation()
    {
        stateManager.StopApproachingPlayer();
    }

    public virtual void OnStateUtilityTimerEnd() { }
}

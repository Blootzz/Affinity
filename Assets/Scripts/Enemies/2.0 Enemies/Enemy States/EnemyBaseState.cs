using UnityEngine;
using System;

public abstract class EnemyBaseState
{
    protected EnemyStateManager stateManager;

    // Constructor sets stateManager
    public EnemyBaseState(EnemyStateManager newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public void SetStateManager(EnemyStateManager newStateManager)
    {
        this.stateManager = newStateManager;
    }
    public virtual void OnEnter()
    {
    }
    public virtual void OnExit()
    {
    }
    public virtual void EndStateByAnimation()
    {
    }
    public virtual void OnPlayerEnteredAttackZone()
    {
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

    public virtual void OnStateUtilityTimerEnd()
    {
    }
}

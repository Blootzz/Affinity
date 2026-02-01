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
        //Debug.Log(stateManager.name + " BaseState.OnEnter");
        if (stateAnimation != null)
        {
            //Debug.Log(stateManager.name + " Playing animation: " + stateAnimation.name);
            stateManager.animator.Play(stateAnimation.name, 0, 0);
        }
        else
            Debug.LogWarning("No animation assigned for " + stateManager.gameObject.name + " state " + stateManager.currentStateName + 
                "\nIgnore if using Generic state that manually attempts to play animation by string name");
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

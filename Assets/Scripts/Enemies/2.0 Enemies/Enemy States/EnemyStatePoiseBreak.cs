using UnityEngine;
using System;

[CreateAssetMenu(menuName = "States/Enemy/Poise Break")]
public class EnemyStatePoiseBreak : EnemyBaseState
{
    //public EnemyStatePoiseBreak(EnemyStateManager newStateManager) : base(newStateManager)
    //{
    //    base.OnEnter();
    //}

    public override void OnEnter()
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "PoiseBreak"))
            stateManager.animator.Play("PoiseBreak", -1, 0);
        else
            Debug.LogError("Does not contain animation \"PoiseBreak\"");
        stateManager.facePlayer.SetEnableAutomaticFlip(true);
        StartFlashing();

        stateManager.SetIsPoiseBroken(true); // logic used for player attack damage bonus
    }
    public override void OnExit()
    {
        base.OnExit();
        stateManager.facePlayer.SetEnableAutomaticFlip(false);
        StopFlashing();

        stateManager.SetIsPoiseBroken(false);
    }

    public override void EndStateByAnimation()
    {
        base.EndStateByAnimation();
        stateManager.SwitchState(stateManager.stateOnExitingPoiseBreak);
    }

    void StartFlashing()
    {
        stateManager.colorFlasher.StartRepeatingBlueFlash(); 
    }
    void StopFlashing()
    {
        stateManager.colorFlasher.EndBlueFlash();
    }
}

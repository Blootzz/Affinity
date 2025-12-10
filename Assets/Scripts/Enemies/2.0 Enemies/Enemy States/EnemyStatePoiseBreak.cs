using UnityEngine;
using System;

public class EnemyStatePoiseBreak : EnemyBaseState
{
    public EnemyStatePoiseBreak(EnemyStateManager newStateManager) : base(newStateManager)
    {
        base.OnEnter();
        if (AnimatorHasClip(stateManager.animator, "PoiseBreak"))
            stateManager.animator.Play("PoiseBreak", -1, 0);
        else
            Debug.LogError("Does not contain animation \"PoiseBreak\"");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateManager.facePlayer.SetManualControllerOn(true);
        StartFlashing();

        stateManager.SetIsPoiseBroken(true);
    }
    public override void OnExit()
    {
        base.OnExit();
        stateManager.facePlayer.SetManualControllerOn(false);
        StopFlashing();

        stateManager.SetIsPoiseBroken(false);
    }

    public override void EndStateByAnimation()
    {
        base.EndStateByAnimation();
        Debug.Log("Switching from poise break to Idle. isAggro: "+stateManager.isAggro);
        stateManager.SwitchState(stateManager.stateOnExitingPoiseBreak);
    }

    void StartFlashing()
    {
        stateManager.GetComponent<ColorFlash>().StartRepeatingBlueFlash(); 
    }
    void StopFlashing()
    {
        stateManager.GetComponent<ColorFlash>().EndFlash();
    }
}

using UnityEngine;
using System;

[CreateAssetMenu(menuName = "States/Enemy/Generic States/Main/Poise Break")]
public class EnemyStatePoiseBreak : EnemyBaseState
{
    public override void OnEnter()
    {
        base.OnEnter();
        if (stateAnimation == null)
        {
            if (AnimatorHasClip(stateManager.animator, "PoiseBreak"))
                stateManager.animator.Play("PoiseBreak", -1, 0);
            else
                Debug.LogError("Does not contain animation \"PoiseBreak\"");
        }

        stateManager.characterMover.SetRbType(RigidbodyType2D.Dynamic);

        stateManager.facePlayer.SetEnableAutomaticFlip(false);
        StartFlashing();

        stateManager.SetIsPoiseBroken(true); // logic used for player attack damage bonus
    }
    public override void OnExit()
    {
        base.OnExit();
        stateManager.facePlayer.SetEnableAutomaticFlip(true);
        StopFlashing();

        stateManager.SetIsPoiseBroken(false);
    }

    public override void EndStateByAnimation()
    {
        base.EndStateByAnimation();
        if (stateManager.stateOnExitingPoiseBreak != null)
            stateManager.SwitchState(stateManager.stateOnExitingPoiseBreak);
        else
            stateManager.SwitchState(stateManager.stateIdle);
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

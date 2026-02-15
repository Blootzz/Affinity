using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/Generic States/Main/Idle")]
public class EnemyStateIdle : EnemyBaseState
{
    [Tooltip("If enabled, will switch straight to action1 when player enters attack zone")]
    [SerializeField] bool onAggroEnterAction1 = true;

    /// <summary>
    /// Plays "Idle" animation if found
    /// </summary>
    public override void OnEnter()
    {
        if (stateAnimation != null)
            return;

        if (AnimatorHasClip(stateManager.animator, "Idle"))
            stateManager.animator.Play("Idle");
        else
            Debug.LogError("Does not contain animation \"Idle\"");
    }

    public virtual void DoNextAction()
    {
        Debug.LogWarning("No next action out of Idle");
    }

    public override void OnPlayerEnteredAttackZone()
    {

        base.OnPlayerEnteredAttackZone();
        //Debug.Log(stateManager.gameObject.name + " calling base.OnPlayerEnteredAttackZone");
        //Debug.Log(stateManager.stateAction1.name);
        if (onAggroEnterAction1)
            stateManager.SwitchState(stateManager.stateAction1);
    }
}

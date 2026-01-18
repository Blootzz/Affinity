using UnityEngine;


[CreateAssetMenu(menuName = "States/Player/DoubleJumping")]
public class PlayerStateDoubleJumping : PlayerStateJumping // inherits from PlayerStateFalling too
{
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void JumpByCharacterJumper()
    {
        stateManager.characterJumper.BeginDoubleJumpAscent();
    }

    public override void JumpEffects()
    {

    }
}
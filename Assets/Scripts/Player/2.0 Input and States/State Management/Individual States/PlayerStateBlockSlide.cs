using UnityEngine;

public class PlayerStateBlockSlide : PlayerStateBlocking
{
    PhysicsMaterialManager physicsMaterialManager;

    public PlayerStateBlockSlide(PlayerStateManager newStateManager) : base(newStateManager)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // base.OnEnter() calls VerticalAxis() to determine which blocker should be up. In BlockSlide, this gives the player the freedom to block up or down while in block slide
        
        physicsMaterialManager = stateManager.GetComponent<PhysicsMaterialManager>();
        ApplySlideVelocity();
    }
    public override void OnExit()
    {
        base.OnExit();
        physicsMaterialManager.SetRbZeroFrictionBounce();
    }
    
    public override void BlockCancel()
    {
        // do nothing, player cannot cancel block while sliding by letting go of block button
    }

    public override void Attack()
    {
        // do nothing, player cannot parry while in block slide

        //persistBlockerUponExit = true;
        //stateManager.SwitchState(new PlayerStateParrying(stateManager));
    }
    
    public override void JumpStart()
    {
        // do nothing, player cannot jump out of blockSlide
        //stateManager.SwitchState(new PlayerStateJumping(stateManager));
    }

    void ApplySlideVelocity()
    {
        physicsMaterialManager.SetRbPlayerBlockSlide();

        // zero velocity
        stateManager.characterMover.SetVelocity(Vector2.zero);

        // GetKnockback already accounts for which direction the knockback should face
        float horizontalSlideVelocity = stateManager.blockParryManager.GetIncomingEnemyHitbox().GetKnockback().x;

        stateManager.characterMover.SetVelocityX(horizontalSlideVelocity);
    }

    // for if BlockSlide sends player off edge
    public override void ProcessGroundCheckEvent(bool isGrounded)
    {
        if (isGrounded == false)
            stateManager.SwitchState(new PlayerStateFalling(stateManager));
        else
            Debug.LogWarning("Running state just received isGrounded is now true???\n" +
                "This is probably due to being spawned in airbourne with no state");
    }

    // stateManager is listening to CharacterMover for Event then calls this
    public override void HorVelocityHitZero()
    {
        stateManager.SwitchState(new PlayerStateIdle(stateManager));
    }
}

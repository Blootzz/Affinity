using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator animator;

    #region Animation Hashes
    // public...  { get; private set; } ensures PlayerState can activate this animation without messing with it
    // Armed
    int Idle;
    int Pant;
    int Run;
    int Stunned;
    int Falling;
    int FallingExt;
    int Reach;
    int Hanging;
    int ClimbLedge;
    int LedgeJump;
    int WallSlide;
    int Crouch;
    // Unarmed
    int IdleUnarmed;
    int PantUnarmed;
    int RunUnarmed;
    int StunnedUnarmed;
    int FallingUnarmed;
    int FallingExtUnarmed;
    int ReachUnarmed;
    int HangingUnarmed;
    int ClimbLedgeUnarmed;
    int LedgeJumpUnarmed;
    int WallSlideUnarmed;
    int CrouchUnarmed;

    // Combat
    public int Block { get; private set; }
    public int BlockUp { get; private set; }
    public int Parry { get; private set; }
    public int ParryUp { get; private set; }
    public int QuickThrow { get; private set; }
    public int StraightAttack { get; private set; }
    public int UpAttack { get; private set; }
    public int DownAttack { get; private set; }
    public int Jab1Animation { get; private set; }
    public int Jab2Animation { get; private set; }
    public int Jab3Animation { get; private set; }
    public int OverheadAnimation { get; private set; }
    public int Throw { get; private set; }
    public int ThrowHeavy { get; private set; }

    // Special Abilities
    // Zipline (used in Rope.cs)
    public int ZiplineForward { get; private set; }
    public int ZiplineBackward { get; private set; }
    //public int ZiplineIdle {get; private set; }
    public int ZipAttackForward { get; private set; }
    public int ZipAttackBackward { get; private set; }
    public int ZiplineApproach { get; private set; }

    // Armed or Unarmed version of animation will keep track of which of the two animations to play (Idle means armed Idle)
    public int AorUIdle { get; private set; }
    public int AorUPant { get; private set; }
    public int AorURun { get; private set; }
    public int AorUStunned { get; private set; }
    public int AorUFalling { get; private set; }
    public int AorUFallingExt { get; private set; }
    public int AorUReach { get; private set; }
    public int AorUHanging { get; private set; }
    public int AorUClimbLedge { get; private set; }
    public int AorULedgeJump { get; private set; }
    public int AorUWallSlide { get; private set; }
    public int AorUCrouch { get; private set; }
    public int DynamicIdle { get; private set; } // changes idle to pant depending on how much health

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EstablishAnimations();
    }

    private void EstablishAnimations() // Relies on naming convention
    {
        // Regular
        Idle = Animator.StringToHash(name + "Idle");
        //Walk = Animator.StringToHash(name + "Walk");
        Pant = Animator.StringToHash(name + "Pant");
        Run = Animator.StringToHash(name + "Run");
        Stunned = Animator.StringToHash(name + "Stunned");
        Falling = Animator.StringToHash(name + "Falling");
        FallingExt = Animator.StringToHash(name + "FallingExt");
        Reach = Animator.StringToHash(name + "Reach");
        Hanging = Animator.StringToHash(name + "Hanging");
        ClimbLedge = Animator.StringToHash(name + "ClimbLedge");
        LedgeJump = Animator.StringToHash(name + "LedgeJump");
        WallSlide = Animator.StringToHash(name + "WallSlide");
        Crouch = Animator.StringToHash(name + "Crouch");

        // Unarmed
        IdleUnarmed = Animator.StringToHash(name + "IdleUnarmed");
        PantUnarmed = Animator.StringToHash(name + "PantUnarmed");
        RunUnarmed = Animator.StringToHash(name + "RunUnarmed");
        StunnedUnarmed = Animator.StringToHash(name + "StunnedUnarmed");
        FallingUnarmed = Animator.StringToHash(name + "FallingUnarmed");
        FallingExtUnarmed = Animator.StringToHash(name + "FallingExtUnarmed");
        ReachUnarmed = Animator.StringToHash(name + "ReachUnarmed");
        HangingUnarmed = Animator.StringToHash(name + "HangingUnarmed");
        ClimbLedgeUnarmed = Animator.StringToHash(name + "ClimbLedgeUnarmed");
        LedgeJumpUnarmed = Animator.StringToHash(name + "LedgeJumpUnarmed");
        WallSlideUnarmed = Animator.StringToHash(name + "WallSlideUnarmed");
        CrouchUnarmed = Animator.StringToHash(name + "CrouchUnarmed");

        // attacks
        Block = Animator.StringToHash(name + "Block");
        BlockUp = Animator.StringToHash(name + "BlockUp");
        Parry = Animator.StringToHash(name + "Parry");
        ParryUp = Animator.StringToHash(name + "ParryUp");
        QuickThrow = Animator.StringToHash(name + "QuickThrow");
        StraightAttack = Animator.StringToHash(name + "StraightAttack");
        UpAttack = Animator.StringToHash(name + "UpAttack");
        DownAttack = Animator.StringToHash(name + "DownAttack");
        Jab1Animation = Animator.StringToHash(name + "Jab1");
        Jab2Animation = Animator.StringToHash(name + "Jab2");
        Jab3Animation = Animator.StringToHash(name + "Jab3");
        OverheadAnimation = Animator.StringToHash(name + "Overhead");
        Throw = Animator.StringToHash(name + "Throw");
        ThrowHeavy = Animator.StringToHash(name + "ThrowHeavy");

        // Special Abilities
        ZiplineForward = Animator.StringToHash(name + "ZiplineForward");
        ZiplineBackward = Animator.StringToHash(name + "ZiplineBackward");
        //ZiplineIdle = Animator.StringToHash("ZiplineForward"); 
        ZipAttackForward = Animator.StringToHash("ZipAttackForward");
        ZipAttackBackward = Animator.StringToHash("ZipAttackBackward");
        ZiplineApproach = Animator.StringToHash("ZiplineApproach");

        SetChangingAnimations();
    }

    /// <summary>
    /// Only used by EstablishAnimations() so that animations default to armed versions
    /// </summary>
    void SetChangingAnimations()
    {
        AorUIdle = Idle;
        AorUPant = Pant;
        AorURun = Run;
        AorUStunned = Stunned;
        AorUFalling = Falling;
        AorUFallingExt = FallingExt;
        AorUReach = Reach;
        AorUHanging = Hanging;
        AorUClimbLedge = ClimbLedge;
        AorULedgeJump = LedgeJump;
        AorUWallSlide = WallSlide;
        AorUCrouch = Crouch;

        DynamicIdle = Idle;
    }

    /// <summary>
    /// To be used whenever the shield is thrown or caught
    /// </summary>
    public void ToggleAorUAnimations()
    {
        if (AorURun != Run) // if variable animations are set to Unarmed
        {
            AorUIdle = Idle;
            AorUPant = Pant;
            AorURun = Run;
            AorUStunned = Stunned;
            AorUFalling = Falling;
            AorUFallingExt = FallingExt;
            AorUReach = Reach;
            AorUHanging = Hanging;
            AorUClimbLedge = ClimbLedge;
            AorULedgeJump = LedgeJump;
            AorUCrouch = Crouch;
        }
        else
        {
            // set all changing animations to Unarmed
            AorUIdle = IdleUnarmed;
            AorUPant = PantUnarmed;
            AorURun = RunUnarmed;
            AorUStunned = StunnedUnarmed;
            AorUFalling = FallingUnarmed;
            AorUFallingExt = FallingExtUnarmed;
            AorUReach = ReachUnarmed;
            AorUHanging = HangingUnarmed;
            AorUClimbLedge = ClimbLedgeUnarmed;
            AorULedgeJump = LedgeJumpUnarmed;
            AorUCrouch = CrouchUnarmed;
        }

        if (DynamicIdle == Pant || DynamicIdle == PantUnarmed)
        {
            DynamicIdle = AorUPant; // Keep panting but with new armed/unarmed condition
        }
        else
        {
            DynamicIdle = AorUIdle; // Keep idling with correct armed/unarmed condition
        }
    }

    /// <summary>
    /// Uses animator reference to play passed animation variable
    /// </summary>
    /// <param name="animationHash">Publicly accessed animation (e.g. PlayerAnimationManager.AorUIdle)</param>
    public void PlayAnimation(int animationHash)
    {
        animator.Play(animationHash);
    }

    /// <summary>
    /// Simply calls animator.SetBool so that the animator can be kept private
    /// </summary>
    /// <param name="animatorParam"></param>
    /// <param name="trueFalse"></param>
    public void AnimatorSetBool(string animatorParam, bool trueFalse)
    {
        animator.SetBool(animatorParam, trueFalse);
    }
}

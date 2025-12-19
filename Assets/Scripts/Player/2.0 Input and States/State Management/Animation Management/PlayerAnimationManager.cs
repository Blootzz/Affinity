using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    Animator animator;
    [SerializeField] string animationPrefix = "Anselm";

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
    int Crouch;
    int PoiseDepleted;
    int SHORYUKEN;
    int WallSlide;
    int WallJump;
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
    int CrouchUnarmed;
    int PoiseDepletedUnarmed;
    int SHORYUKENUnarmed;
    int WallSlideUnarmed;
    int WallJumpUnarmed;

    // Combat
    public int Block { get; private set; }
    public int BlockUp { get; private set; }
    public int Parry { get; private set; }
    public int ParryUp { get; private set; }
    public int QuickThrow { get; private set; }
    public int StraightAttack { get; private set; }
    public int UpAttack { get; private set; }
    public int Throw { get; private set; }

    // Special Abilities
    // Zipline (used in Rope.cs)
    public int ZiplineForward { get; private set; }
    public int ZiplineBackward { get; private set; }
    //public int ZiplineIdle {get; private set; }
    public int ZipAttackForward { get; private set; }
    public int ZipAttackBackward { get; private set; }
    public int ZiplineApproach { get; private set; }

    // Armed or Unarmed version of animation will keep track of which of the two animations to play (Idle means armed Idle)
    public int DynamicIdle { get; private set; } // changes idle to pant depending on how much health
    int AorUIdle { get; set; }
    int AorUPant { get; set; }
    public int AorURun { get; private set; }
    public int AorUStunned { get; private set; }
    public int AorUFalling { get; private set; }
    public int AorUFallingExt { get; private set; }
    public int AorUReach { get; private set; }
    public int AorUHanging { get; private set; }
    public int AorUClimbLedge { get; private set; }
    public int AorULedgeJump { get; private set; }
    public int AorUCrouch { get; private set; }
    public int AorUPoiseDepleted { get; private set; }
    public int AorUSHORYUKEN { get; private set; }
    public int AorUWallSlide { get; private set; }
    public int AorUWallJump { get; private set; }

    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EstablishAnimations();
    }

    private void EstablishAnimations() // Relies on naming convention
    {
        // Regular
        Idle = Animator.StringToHash(animationPrefix + "Idle");
        Pant = Animator.StringToHash(animationPrefix + "Pant");
        Run = Animator.StringToHash(animationPrefix + "Run");
        Stunned = Animator.StringToHash(animationPrefix + "Stunned");
        Falling = Animator.StringToHash(animationPrefix + "Falling");
        FallingExt = Animator.StringToHash(animationPrefix + "FallingExt");
        Reach = Animator.StringToHash(animationPrefix + "Reach");
        Hanging = Animator.StringToHash(animationPrefix + "Hanging");
        ClimbLedge = Animator.StringToHash(animationPrefix + "ClimbLedge");
        LedgeJump = Animator.StringToHash(animationPrefix + "LedgeJump");
        Crouch = Animator.StringToHash(animationPrefix + "Crouch");
        PoiseDepleted = Animator.StringToHash(animationPrefix + "PoiseDepleted");
        SHORYUKEN = Animator.StringToHash("SHORYUKEN");
        WallSlide = Animator.StringToHash(animationPrefix + "WallSlide");
        WallJump = Animator.StringToHash(animationPrefix + "WallJump");

        // Unarmed
        IdleUnarmed = Animator.StringToHash(animationPrefix + "IdleUnarmed");
        PantUnarmed = Animator.StringToHash(animationPrefix + "PantUnarmed");
        RunUnarmed = Animator.StringToHash(animationPrefix + "RunUnarmed");
        StunnedUnarmed = Animator.StringToHash(animationPrefix + "StunnedUnarmed");
        FallingUnarmed = Animator.StringToHash(animationPrefix + "FallingUnarmed");
        FallingExtUnarmed = Animator.StringToHash(animationPrefix + "FallingExtUnarmed");
        ReachUnarmed = Animator.StringToHash(animationPrefix + "ReachUnarmed");
        HangingUnarmed = Animator.StringToHash(animationPrefix + "HangingUnarmed");
        ClimbLedgeUnarmed = Animator.StringToHash(animationPrefix + "ClimbLedgeUnarmed");
        LedgeJumpUnarmed = Animator.StringToHash(animationPrefix + "LedgeJumpUnarmed");
        CrouchUnarmed = Animator.StringToHash(animationPrefix + "CrouchUnarmed");
        PoiseDepletedUnarmed = Animator.StringToHash(animationPrefix + "PoiseDepletedUnarmed"); 
        SHORYUKENUnarmed = Animator.StringToHash("SHORYUKENUnarmed");
        WallSlideUnarmed = Animator.StringToHash(animationPrefix + "WallSlideUnarmed");
        WallJumpUnarmed = Animator.StringToHash(animationPrefix + "WallJumpUnarmed");

        // attacks
        Block = Animator.StringToHash(animationPrefix + "Block");
        BlockUp = Animator.StringToHash(animationPrefix + "BlockUp");
        Parry = Animator.StringToHash(animationPrefix + "Parry");
        ParryUp = Animator.StringToHash(animationPrefix + "ParryUp");
        QuickThrow = Animator.StringToHash(animationPrefix + "QuickThrow");
        StraightAttack = Animator.StringToHash(animationPrefix + "StraightAttack");
        UpAttack = Animator.StringToHash(animationPrefix + "UpAttack");
        Throw = Animator.StringToHash(animationPrefix + "Throw");

        // Special Abilities
        ZiplineForward = Animator.StringToHash(animationPrefix + "ZiplineForward");
        ZiplineBackward = Animator.StringToHash(animationPrefix + "ZiplineBackward");
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
        AorUCrouch = Crouch;
        AorUPoiseDepleted = PoiseDepleted;
        AorUSHORYUKEN = SHORYUKEN;
        AorUWallSlide = WallSlide;
        AorUWallJump = WallJump;

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
            AorUPoiseDepleted = PoiseDepleted;
            AorUSHORYUKEN = SHORYUKEN;
            AorUWallSlide = WallSlide;
            AorUWallJump = WallJump;
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
            AorUPoiseDepleted = PoiseDepletedUnarmed;
            AorUSHORYUKEN = SHORYUKENUnarmed;
            AorUWallSlide = WallSlideUnarmed;
            AorUWallJump = WallJumpUnarmed;
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

    public void PlayAnimationFromStart(int animationHash)
    {
        animator.Play(animationHash, -1, 0);
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

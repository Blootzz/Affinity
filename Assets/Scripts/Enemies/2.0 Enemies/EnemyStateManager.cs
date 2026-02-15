using UnityEngine;
using System;
using System.Collections;
using UnityEditor.ShaderKeywordFilter;

public class EnemyStateManager : MonoBehaviour
{
    [Header("Specific States by Condition")]
    public EnemyBaseState starterState;
    public EnemyBaseState stateOnExitingPoiseBreak;

    [Header("Standard States")]
    public EnemyStateIdle stateIdle;
    public EnemyStateWalk stateWalk;
    public EnemyBaseState stateAction1;
    public EnemyBaseState stateAction2;
    public EnemyBaseState stateAction3;
    public EnemyStatePoiseBreak statePoiseBreak;
    public string currentStateName;

    EnemyBaseState currentState;

    [Header("Object References")]
    [HideInInspector] public Health health;
    [HideInInspector] public Poise poise;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [SerializeField] EnemyHitboxManager hitboxManager; // accessed by EnemyStateAttackBase to be enabled
    [HideInInspector] public Animator animator;
    [SerializeField] DetectZoneByTag agroZone;
    [HideInInspector] public CharacterMover characterMover;
    [HideInInspector] public FacePlayer facePlayer;
    [HideInInspector] public ColorFlash colorFlasher;
    GameObject playerObj;

    // Scriptable objects need this to access/hold data that changes in runtime
    [Header("Flags and Data")]
    public bool isAggro = false;
    public bool isPoiseBroken = false;
    [Tooltip("Multiplies incoming damage when enemy is in Poise Break state")]
    [SerializeField] float poiseBreakDamageMultiplier = 5;
    [Tooltip("Used by classes such as HammerStateAttack2 to keep track of repetitions")]
    public int repeatStateCounter = 0;

    //[InspectorButton(nameof(OnButtonClicked1))]
    //public bool Idle;
    //private void OnButtonClicked1()
    //{
    //    Time.timeScale = 1;
    //    SwitchState(stateIdle);
    //}
    //[InspectorButton(nameof(OnButtonClicked2))]
    //public bool Attack;
    //private void OnButtonClicked2()
    //{
    //    Time.timeScale = .25f;
    //    SwitchState(stateAttack1);
    //}

    void Awake()
    {
        health = GetComponent<Health>();
        poise = GetComponent<Poise>();
        hurtboxManager = GetComponentInChildren<HurtboxManager>();
        animator = GetComponent<Animator>();
        colorFlasher = GetComponent<ColorFlash>();
        //if (agroZone == null)
        //    Debug.LogWarning("Please drag and drop DetectZoneByTag reference to this script");
        if (TryGetComponent(out CharacterMover cm))
            characterMover = cm;
        if (TryGetComponent(out FacePlayer fPlayer))
            facePlayer = fPlayer;
    }

    private void Start()
    {
        SwitchState(starterState);
    }

    private void OnEnable()
    {
        if (hurtboxManager != null)
            hurtboxManager.HurtEvent += OnHurtboxHit;
        health.DeathEvent += OnDeath;
        poise.PoiseDepletedEvent += OnPoiseDepleted;
        if (agroZone != null)
        {
            agroZone.TargetFoundEvent += OnPlayerEnteredAgroZone;
            agroZone.TargetExitedEvent += OnPlayerExitedAgroZone;
        }
    }
    private void OnDisable()
    {
        if (hurtboxManager != null)
            hurtboxManager.HurtEvent -= OnHurtboxHit;
        health.DeathEvent -= OnDeath;
        poise.PoiseDepletedEvent -= OnPoiseDepleted;
        if (agroZone != null)
        {
            agroZone.TargetFoundEvent -= OnPlayerEnteredAgroZone;
            agroZone.TargetExitedEvent -= OnPlayerExitedAgroZone;
        }
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (newState == null)
        {
            Debug.LogError(gameObject.name + " trying to switch to null state out of " +currentStateName);
            return;
        }

        //print(gameObject.name + " switching to " + newState.name);

        if (currentState != null)
            currentState.OnExit();
        currentState = newState;
        currentStateName = newState.GetType().Name;
        currentState.SetStateManager(this);
        currentState.OnEnter();
    }

    // ================================================== Event Listeners ======================================================
    /// <summary>
    /// Deducts health by (incoming hitbox damage) x poiseBreakDamageMultiplier if applicable
    /// </summary>
    void OnHurtboxHit()
    {
        float damageMultiplier = 1;
        if (isPoiseBroken)
        {
            damageMultiplier = poiseBreakDamageMultiplier;
            if (damageMultiplier == 0)
                Debug.LogWarning("damageMultiplier = 0. Will not apply any damage on this hit");
        }

        health.DeductHealth(hurtboxManager.GetIncomingPlayerHitbox().GetDamage() * damageMultiplier);
        colorFlasher.StartSingleWhiteFlash();

        // putting this after health.DeductHealth just in case health logic needs to happen before state switch
        if (isPoiseBroken)
            SwitchState(stateIdle);
    }
    void OnDeath()
    {
        Destroy(this.transform.gameObject);
    }
    void OnPoiseDepleted()
    {
        SwitchState(statePoiseBreak);
    }

    void OnPlayerEnteredAgroZone(GameObject pObj)
    {
        playerObj = pObj;
        // update FacePlayer data before any state change
        facePlayer.OnPlayerEnteredAttackZone(pObj);
        currentState.OnPlayerEnteredAttackZone();
        isAggro = true;
    }
    void OnPlayerExitedAgroZone(GameObject pObj)
    {
        currentState.OnPlayerExitedAttackZone();
        isAggro = true;
    }

    // ============================================= Executive Methods ======================================
    /// <summary>
    /// Calls SlideTowardPlayer, passes player transform
    /// </summary>
    /// <param name="strength"> used as 3rd Lerp parameter</param>
    public void ApproachPlayer(float strength)
    {
        GetComponent<SlideTowardPlayer>().BeginSlide(playerObj.transform, strength);
    }
    public void StopApproachingPlayer()
    {
        GetComponent<SlideTowardPlayer>().EndSlide();
    }

    //======================================= Animation Events ======================================
    public void ANIM_ApproachPlayer() => currentState.BeginLerpToPlayerByAnimation();
    public void ANIM_StopApproachingPlayer() => currentState.EndLerpToPlayerByAnimation();
    public void ANIM_EndStateByAnimation() => currentState.EndStateByAnimation();
    public void ANIM_MidAnimationEvent() => currentState.MidAnimationEvent();

    // ================================================== Timer ===================================================
    bool isTimerBusy = false;
    public void BeginStateUtilityTimer(float seconds)
    {
        if (isTimerBusy)
        {
            Debug.LogWarning("Attempting to use Enemy State timer while already in use");
            return;
        }

        isTimerBusy = true;
        StartCoroutine(Timer(seconds));
    }
    IEnumerator Timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnStateUtilityTimerEnd();
    }
    void OnStateUtilityTimerEnd()
    {
        isTimerBusy = false;
        currentState.OnStateUtilityTimerEnd();
    }


    /// <summary>
    /// tracked in EnemyStateManager to determine how to process incoming hits for poise break vulnerability
    /// </summary>
    /// <param name="newValue"></param>
    public void SetIsPoiseBroken(bool newValue) => isPoiseBroken = newValue;

    public EnemyHitboxManager GetHitboxManager() => hitboxManager;
}

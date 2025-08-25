using UnityEngine;
using System;

public class EnemyStateManager : MonoBehaviour
{
    [HideInInspector] public Health health;
    [HideInInspector] public Poise poise;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [HideInInspector] public Animator animator;
    [SerializeField] DetectZoneByTag attackDetectZone;

    [SerializeField] string startingScriptName;
    EnemyBaseState currentState;
    [SerializeField] string currentStateName;
    GameObject playerObj;
    public bool isAggro = false;

    [InspectorButton(nameof(OnButtonClicked1))]
    public bool Idle;
    private void OnButtonClicked1()
    {
        Time.timeScale = 1;
        SwitchState(new EnemyStateIdle(this));
    }
    [InspectorButton(nameof(OnButtonClicked2))]
    public bool Attack;
    private void OnButtonClicked2()
    {
        Time.timeScale = .25f;
        SwitchState(new EnemyStateAttack1(this));
    }

    void Awake()
    {
        health = GetComponent<Health>();
        poise = GetComponent<Poise>();
        hurtboxManager = GetComponentInChildren<HurtboxManager>();
        animator = GetComponent<Animator>();
        if (attackDetectZone == null)
            Debug.LogWarning("Please drag and drop DetectZoneByTag reference to this script");
    }

    private void Start()
    {
        //currentState = new EnemyStateIdle(this);
        //currentState = startingState;
        //currentState.OnEnter();
        //SwitchState(startingState);
    }

    private void OnEnable()
    {
        hurtboxManager.HurtEvent += OnHurtboxHit;
        health.DeathEvent += OnDeath;
        poise.PoiseDepletedEvent += OnPoiseDepleted;
        attackDetectZone.TargetFoundEvent += OnPlayerEnteredAttackZone;
    }
    private void OnDisable()
    {
        hurtboxManager.HurtEvent -= OnHurtboxHit;
        health.DeathEvent -= OnDeath;
        poise.PoiseDepletedEvent -= OnPoiseDepleted;
        attackDetectZone.TargetFoundEvent -= OnPlayerEnteredAttackZone;
    }

    void OnHurtboxHit()
    {
        health.DeductHealth(hurtboxManager.GetIncomingPlayerHitbox().GetDamage());
    }
    void OnDeath()
    {
        Destroy(this.transform.parent.gameObject);
    }
    void OnPoiseDepleted()
    {
        print("implement enemy poise depleted here");
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = newState;
        currentStateName = newState.GetType().Name;
        currentState.SetStateManager(this);
        currentState.OnEnter();
    }

    void OnPlayerEnteredAttackZone(GameObject pObj)
    {
        playerObj = pObj;
        currentState.OnPlayerEnteredAttackZone();
        isAggro = true;
    }

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
    public void ANIM_ApproachPlayer()
    {
        currentState.BeginLerpToPlayerByAnimation();
    }
    public void ANIM_StopApproachingPlayer()
    {
        currentState.EndLerpToPlayerByAnimation();
    }

    public void ANIM_EndStateByAnimation()
    {
        currentState.EndStateByAnimation();
    }

    public void BeginStateUtilityTimer(float seconds)
    {
        print("start new coroutine here that calls currentState.OnStateUtilityTimerEnd()");
    }
}

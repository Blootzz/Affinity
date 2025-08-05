using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [HideInInspector] public Health health;
    [HideInInspector] public Poise poise;
    [HideInInspector] public HurtboxManager hurtboxManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] DetectZoneByTag attackDetectZone;
    
    EnemyBaseState currentState;
    [SerializeField] string currentStateName;
    GameObject playerObj;

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
        attackDetectZone = GetComponentInChildren<DetectZoneByTag>();
    }

    private void Start()
    {
        currentState = new EnemyStateIdle(this);
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
        currentState?.OnExit();
        currentState = newState;
        currentStateName = newState.GetType().Name;
        currentState.OnEnter();
    }

    void OnPlayerEnteredAttackZone(GameObject pObj)
    {
        playerObj = pObj;
        SwitchState(new EnemyStateAttack1(this));
    }

    public void ANIM_EndStateByAnimation()
    {
        currentState.EndStateByAnimation();
    }
}

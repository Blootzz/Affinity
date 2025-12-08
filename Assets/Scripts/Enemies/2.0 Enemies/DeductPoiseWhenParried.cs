using UnityEngine;

public class DeductPoiseWhenParried : MonoBehaviour
{
    [SerializeField] EnemyHitboxManager hitboxManager;
    Poise enemyPoise;

    private void Start()
    {
        enemyPoise = GetComponent<Poise>();
    }

    void OnEnable()
    {
        hitboxManager.HitboxParriedEvent += OnHitboxParriedDeductPoise;
    }
    void OnDisable()
    {
        hitboxManager.HitboxParriedEvent -= OnHitboxParriedDeductPoise;
    }

    void OnHitboxParriedDeductPoise(EnemyHitbox enemyHitbox)
    {
        enemyPoise.EnemyDeductPoiseFromPlayerParry(enemyHitbox.GetDamage());
    }
}

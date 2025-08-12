using UnityEngine;
using System;

public class HurtboxManager : MonoBehaviour
{
    public event Action HurtEvent;
    GameObject incomingHitbox;
    bool temporaryDisable = false; // used to prevent double on same frame when blocker was hit first

    // default for enemies
    [SerializeField] bool canBeHitByPlayer = true;
    [SerializeField] bool canBeHitByEnemy = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (temporaryDisable)
            return;

        if (collision.gameObject.GetComponent<BaseHitbox>())
        {
            print("Hurtbox collision");
            incomingHitbox = collision.gameObject;

            if (canBeHitByPlayer && incomingHitbox.GetComponent<PlayerHitbox>())
            {
                HurtEvent?.Invoke();
                return;
            }
            if (canBeHitByEnemy && incomingHitbox.GetComponent<EnemyHitbox>())
            {
                HurtEvent?.Invoke();
                return;
            }
        }
    }

    public EnemyHitbox GetIncomingEnemyHitbox()
    {
        return incomingHitbox.GetComponent<EnemyHitbox>();
    }

    public PlayerHitbox GetIncomingPlayerHitbox()
    {
        return incomingHitbox.GetComponent<PlayerHitbox>();
    }

    public void DisableOneFrame()
    {
        temporaryDisable = true;
    }
    private void LateUpdate()
    {
        temporaryDisable = false;
    }
}

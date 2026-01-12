using UnityEngine;
using System;

public class HurtboxManager : MonoBehaviour
{
    public event Action HurtEvent;
    GameObject incomingHitbox;
    bool temporaryDisable = false; // used to prevent double on same frame when blocker was hit first
    bool invulnerabilityOn = false;

    // default for enemies
    [SerializeField] bool canBeHitByPlayer = true;
    [SerializeField] bool canBeHitByEnemy = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (temporaryDisable)
            return;
        if (invulnerabilityOn)
            return;

        if (collision.gameObject.GetComponent<BaseHitbox>())
        {
            incomingHitbox = collision.gameObject;

            if (canBeHitByPlayer && incomingHitbox.GetComponent<PlayerHitbox>())
            {
                HurtEvent?.Invoke();
                return;
            }
            else if (canBeHitByEnemy && incomingHitbox.GetComponent<EnemyHitbox>())
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

    public void SetInvulnerability(bool newInvulnerability)
    {
        invulnerabilityOn = newInvulnerability;
    }
}

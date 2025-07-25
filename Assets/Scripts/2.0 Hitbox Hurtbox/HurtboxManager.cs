using UnityEngine;
using System;

public class HurtboxManager : MonoBehaviour
{
    public event Action HurtEvent;
    EnemyHitbox incomingHitbox;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyHitbox enemyHBox))
        {
            incomingHitbox = enemyHBox;
            HurtEvent?.Invoke();
        }
    }

    public EnemyHitbox GetIncomingHitbox()
    {
        return incomingHitbox;
    }
}

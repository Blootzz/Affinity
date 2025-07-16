using UnityEngine;
using System;

public class BlockParryManager : MonoBehaviour
{
    public event Action<EnemyHitbox> BlockerHitEvent;

    public void FireBlockerHitEvent(EnemyHitbox enemyHitbox)
    {
        BlockerHitEvent?.Invoke(enemyHitbox);
    }
}

using UnityEngine;
using System;

public class BlockParryManager : MonoBehaviour
{
    public event Action<EnemyHitbox, bool> BlockerHitEvent;
    [SerializeField] bool isParryWindowOpen = false; // modified by animation

    public void FireBlockerHitEvent(EnemyHitbox enemyHitbox)
    {
        BlockerHitEvent?.Invoke(enemyHitbox, isParryWindowOpen);
    }

    public void ClearIsParryWindowOpen()
    {
        isParryWindowOpen = false;
    }
}

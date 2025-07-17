using UnityEngine;
using System;

public class BlockParryManager : MonoBehaviour
{
    public event Action BlockerHitEvent;
    [SerializeField] bool isParryWindowOpen = false; // modified by animation
    EnemyHitbox incomingEnemyHitbox;

    public void FireBlockerHitEvent(EnemyHitbox enemyHitbox)
    {
        incomingEnemyHitbox = enemyHitbox;
        BlockerHitEvent?.Invoke();
    }

    public bool GetIsParryWindowOpen()
    {
        return isParryWindowOpen;
    }

    public EnemyHitbox GetIncomingEnemyHitbox()
    {
        return incomingEnemyHitbox;
    }

    public void ClearIsParryWindowOpen()
    {
        isParryWindowOpen = false;
    }
}

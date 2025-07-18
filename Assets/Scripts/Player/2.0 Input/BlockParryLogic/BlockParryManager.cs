using UnityEngine;
using System;

public class BlockParryManager : MonoBehaviour
{
    public event Action BlockerHitEvent;
    [SerializeField] bool isParryWindowOpen = false; // modified by animation
    EnemyHitbox incomingEnemyHitbox;
    [SerializeField] BlockParryCollider lowerCollider;
    [SerializeField] BlockParryCollider upperCollider;

    private void Awake()
    {
        if (lowerCollider == null || upperCollider == null)
            Debug.LogError("Please drag and drop lower and upper collider references into BlockParryManager");
    }

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

    public void EnableBlockerLower(bool enableMePlz)
    {
        lowerCollider.enabled = enableMePlz;
    }
    public void EnableBlockerUpper(bool enableMePlz)
    {
        upperCollider.enabled = enableMePlz;
    }
}

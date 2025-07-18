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

    /// <summary>
    /// Used to control enabled state of lower and upper block colliders by GameObject.SetActive
    /// </summary>
    /// <param name="enableLower"></param>
    /// <param name="enableUpper"></param>
    public void SetEnableBlockers(bool enableLower, bool enableUpper)
    {
        print("Setting blockers to: " + enableLower + " and " + enableUpper);
        lowerCollider.gameObject.SetActive(enableLower);
        upperCollider.gameObject.SetActive(enableUpper);
    }
}

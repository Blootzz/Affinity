using UnityEngine;
using System;
using System.Collections;

public class BlockParryManager : MonoBehaviour
{
    public event Action BlockerHitEvent; // Listened to by PlayerStateManager
    [SerializeField] bool isParryWindowOpen = false; // no longer modified directly by animation, but set by ANIM events
    EnemyHitbox incomingEnemyHitbox;
    [SerializeField] BlockParryCollider lowerCollider;
    [SerializeField] BlockParryCollider upperCollider;

    [SerializeField] ParticleSystem blockParticles;
    [SerializeField] GameObject blockWaveEffect;
    [SerializeField] GameObject parryEffect;

    Vector2 visualEffectSpawnPosition;

    [Header("Parry SloMo")]
    [SerializeField] TimeManager timeManager;
    [SerializeField] float SloMoDurationUnscaled = 1f;
    [SerializeField][Range(0, 1)] float SloMoInitialTimeScale = 0.05f;

    LensZoomEffect lensZoomEffect;

    private void Awake()
    {
        if (lowerCollider == null || upperCollider == null)
            Debug.LogError("Please drag and drop lower and upper collider references into BlockParryManager");
    }

    private void Start()
    {
        lensZoomEffect = FindFirstObjectByType<LensZoomEffect>();
    }

    public void FireBlockerHitEvent(EnemyHitbox enemyHitbox, Vector2 blockerEffectWorldPosition)
    {
        incomingEnemyHitbox = enemyHitbox;
        // State calls CreateVisualEffect so it can pass in faceRight and bool parryInsteadOfBlock
        visualEffectSpawnPosition = blockerEffectWorldPosition;

        BlockerHitEvent?.Invoke();
    }

    public bool GetIsParryWindowOpen()
    {
        return isParryWindowOpen;
    }
    public void SetIsParryWindowOpen(bool open)
    {
        isParryWindowOpen = open;
    }

    public EnemyHitbox GetIncomingEnemyHitbox()
    {
        return incomingEnemyHitbox;
    }

    /// <summary>
    /// usually modified by animation, but this is a manual option in case parry animation is disrupted
    /// </summary>
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
        lowerCollider.gameObject.SetActive(enableLower);
        upperCollider.gameObject.SetActive(enableUpper);
    }


    /// <summary>
    /// Animator manipulates hitbox GameObject.SetActive(t/f). Trying to manipulate that doesn't work.
    /// This just disables collider. Also changes hitbox color
    /// </summary>
    public void DisableHitboxCollider()
    {
        GetIncomingEnemyHitbox().RelayHitboxLandedToManager();
    }

    public void ParryEnemy()
    {
        GetIncomingEnemyHitbox().GetParried();
    }

    /// <summary>
    /// Spawn parry or block effect at BlockParryCollider spawn point
    /// </summary>
    /// <param name="faceRight"></param>
    /// <param name="parryInsteadOfBlock"></param>
    public void CreateVisualEffect(bool faceRight, bool parryInsteadOfBlock)
    {
        // instantiate effect based off blocker's spawn position, rotate with faceRight
        if (parryInsteadOfBlock)
            Instantiate(parryEffect, visualEffectSpawnPosition, Quaternion.Euler(new Vector3(0, faceRight? 0:180, 0)));
        else
            Instantiate(blockWaveEffect, visualEffectSpawnPosition, Quaternion.Euler(new Vector3(0, faceRight? 0:180, 0)));

        //Vector2 angleVector = new Vector2(collision.gameObject.transform.position.x - localPos.x, collision.gameObject.transform.position.y - localPos.y);
        //float angleDeg = 180 / Mathf.PI * Mathf.Atan(angleVector.y / angleVector.x);

        //Instantiate(blockWaveEffect, Vector3.zero, Quaternion.Euler(new Vector3(0, 0/*thePlayer.faceRight? 0:180*/, 0 /*angleDeg*/)), this.transform);

    }

    public void StartSloMo()
    {
        timeManager.SetTimeScale(SloMoInitialTimeScale);
        StartCoroutine(IncrementTimeScale());
    }
    IEnumerator IncrementTimeScale()
    {
        yield return new WaitForSecondsRealtime(SloMoDurationUnscaled);
        timeManager.SetTimeScale(1);
    }

    public void StartZoomEffect()
    {
        lensZoomEffect.BeginParryZoomAnimation();
    }
}

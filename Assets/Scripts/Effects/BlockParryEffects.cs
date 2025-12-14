using UnityEngine;
using System.Collections;

public class BlockParryEffects : MonoBehaviour
{
    BlockParryManager bpManager;

    [SerializeField] GameObject blockWaveEffect;
    [SerializeField] GameObject parryEffect;
    [SerializeField] GameObject blockSlideDust;

    [Header("Parry SloMo")]
    [SerializeField] TimeManager timeManager;
    [SerializeField] float SloMoDurationUnscaled = 1f;
    [SerializeField][Range(0, 1)] float SloMoInitialTimeScale = 0.05f;

    LensZoomEffect lensZoomEffect;
    CamShake camShake;

    private void Awake()
    {
        bpManager = GetComponent<BlockParryManager>();
    }

    private void OnEnable()
    {
        bpManager.SuccessfulBlockEvent += BlockEffects;
        bpManager.SuccessfulParryEvent += ParryEffects;
    }
    private void OnDisable()
    {
        bpManager.SuccessfulBlockEvent -= BlockEffects;
        bpManager.SuccessfulParryEvent -= ParryEffects;
    }

    private void Start()
    {
        lensZoomEffect = FindFirstObjectByType<LensZoomEffect>();
        camShake = FindFirstObjectByType<CamShake>();
    }

    void BlockEffects(bool faceRight)
    {
        CreateVisualEffect(faceRight, false);
        StartCameraShakeEffect();
        EnableBlockSlideDust();
    }
    
    void ParryEffects(bool faceRight)
    {
        CreateVisualEffect(faceRight, true);
        StartSloMo();
        StartZoomEffect();
    }

    //==================================================================

    /// <summary>
    /// Spawn parry or block effect at BlockParryCollider spawn point
    /// </summary>
    /// <param name="faceRight"></param>
    /// <param name="parryInsteadOfBlock"></param>
    public void CreateVisualEffect(bool faceRight, bool parryInsteadOfBlock)
    {
        // instantiate effect based off blocker's spawn position, rotate with faceRight
        if (parryInsteadOfBlock)
            Instantiate(parryEffect, bpManager.visualEffectSpawnPosition, Quaternion.Euler(new Vector3(0, faceRight ? 0 : 180, 0)));
        else
            Instantiate(blockWaveEffect, bpManager.visualEffectSpawnPosition, Quaternion.Euler(new Vector3(0, faceRight ? 0 : 180, 0)));

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
    public void StartCameraShakeEffect()
    {
        camShake.BeginCameraShake(bpManager.GetIncomingEnemyHitbox().GetDamage());
    }

    void EnableBlockSlideDust()
    {
        blockSlideDust.SetActive(true);
    }
}

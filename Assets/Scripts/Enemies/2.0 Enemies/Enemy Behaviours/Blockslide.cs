using UnityEngine;
using System;

public class Blockslide : MonoBehaviour
{
    [SerializeField] EnemyHitboxManager hitboxManager;
    
    float slideDistance;
    [SerializeField] float slideSpeed;
    [Tooltip("Multiplied by enemyHitbox damage")]
    [SerializeField] float slideDistanceMultiplier;

    CharacterMover characterMover;
    FacePlayer facePlayer;
    SlideTowardPlayer slideTowardPlayer;

    void Awake()
    {
        characterMover = GetComponent<CharacterMover>();
        facePlayer = GetComponent<FacePlayer>();
        slideTowardPlayer = GetComponent<SlideTowardPlayer>();
    }

    void OnEnable()
    {
        hitboxManager.HitboxParriedEvent += OnHitboxParried;
    }
    void OnDisable()
    {
        hitboxManager.HitboxParriedEvent -= OnHitboxParried;
    }

    public void OnHitboxParried(EnemyHitbox enHitbox)
    {
        slideDistance = enHitbox.GetDamage() / 20 * slideDistanceMultiplier;
        SlideBackwards();
    }

    /// <summary>
    /// Sets enemy rb to dynamic so that root motion is disabled and the enemy is physically knocked back
    /// The end of the attack animation will set rb to kinematic in EnemyStateAttackBase.cs
    /// </summary>
    void SlideBackwards()
    {
        characterMover.SetRbType(RigidbodyType2D.Dynamic);
        slideTowardPlayer.BeginSlide(slideDistance, facePlayer.GetFaceRight(), slideSpeed);
        //slideTowardPlayer.BeginSlide(10, facePlayer.GetFaceRight()); // force not working, not sure why
    }
}

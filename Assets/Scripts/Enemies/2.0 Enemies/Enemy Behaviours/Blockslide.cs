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

    void SlideBackwards()
    {
        // this is not going to work when enemy overrides the velocity by entering a new state
        //print("Figure out a new way to manage enemy velocity");
        //characterMover.SetMoveSpeed(slideSpeed);
        //characterMover.SetHorizontalMovementVelocity(facePlayer.GetFaceRight() ? -1 : 1);
        //print("Kinematic rb not affected by friction. Use different approach with SlideTowardPlayer");

        slideTowardPlayer.BeginSlide(slideDistance, facePlayer.GetFaceRight(), slideSpeed);
    }
}

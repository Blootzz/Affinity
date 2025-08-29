using UnityEngine;
using System;

public class Blockslide : MonoBehaviour
{
    [SerializeField] EnemyHitboxManager hitboxManager;
    [Tooltip("Multiply by hitbox damage to get slide velocity")]
    [SerializeField] float slideStrength;
    CharacterMover characterMover;
    FacePlayer facePlayer;

    void Awake()
    {
        characterMover = GetComponent<CharacterMover>();
        facePlayer = GetComponent<FacePlayer>();
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
        slideStrength = enHitbox.GetDamage();
        SlideBackwards();
    }

    void SlideBackwards()
    {
        // this is not going to work when enemy overrides the velocity by entering a new state
        print("Figure out a new way to manage enemy velocity");
        characterMover.SetMoveSpeed(slideStrength);
        characterMover.SetHorizontalMovementVelocity(facePlayer.GetFaceRight() ? -1 : 1);
    }
}

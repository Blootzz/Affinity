using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    //[SerializeField] DetectZoneByTag attackDetectZone;
    Transform playerTransform;
    bool playerIsRightOfEnemy = false;
    bool faceRight = false;
    bool isTrackingPlayer = false;

    bool enableAutomaticFlip = true; // used by enemy states so enemy doesn't have perfect aim

    void Awake()
    {
    }

    //void OnEnable()
    //{
    //    attackDetectZone.TargetFoundEvent += OnPlayerEnteredAttackZone;
    //}
    //void OnDisable()
    //{
    //    attackDetectZone.TargetFoundEvent -= OnPlayerEnteredAttackZone;
    //}

    /// <summary>
    /// Called before state swtich so that FacePlayer data is accurate before switching states
    /// </summary>
    public void OnPlayerEnteredAttackZone(GameObject pObj)
    {
        playerTransform = pObj.transform;
        isTrackingPlayer = true;
    }

    void Update()
    {
        PerFrameLogic();
    }

    void PerFrameLogic()
    {
        if (isTrackingPlayer == false)
            return;
        if (!enableAutomaticFlip)
            return;

        if (playerTransform.position.x > transform.position.x)
            playerIsRightOfEnemy = true;
        else
            playerIsRightOfEnemy = false;

        if (faceRight != playerIsRightOfEnemy)
            Flip();
    }

    void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(Vector3.up * 180);
    }

    public void SetEnableAutomaticFlip(bool canFlip)
    {
        enableAutomaticFlip = canFlip;
    }

    public void OneTimeCheck()
    {
        PerFrameLogic();
        print("isTrackingPlayer: " + isTrackingPlayer);
        print("enableAutomaticFlip: " + enableAutomaticFlip);
        print("playerIsRightOfEnemy: " + playerIsRightOfEnemy);
        print("playerIsRightOfEnemy: " + playerIsRightOfEnemy);
        print("faceRight: " + faceRight);
    }

    public bool GetFaceRight()
    {
        return faceRight;
    }
}

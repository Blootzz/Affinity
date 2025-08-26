using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] DetectZoneByTag attackDetectZone;
    Transform playerTransform;
    bool playerIsRightOfEnemy = false;
    bool faceRight = false;
    bool isTrackingPlayer = false;

    bool manualControllerEnabled = true; // used by enemy states so enemy doesn't have perfect aim

    void Awake()
    {
    }

    void OnEnable()
    {
        attackDetectZone.TargetFoundEvent += OnPlayerEnteredAttackZone;
    }
    void OnDisable()
    {
        attackDetectZone.TargetFoundEvent -= OnPlayerEnteredAttackZone;
    }

    void OnPlayerEnteredAttackZone(GameObject pObj)
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
        if (!manualControllerEnabled)
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

    public void SetManualControllerOn(bool manController)
    {
        manualControllerEnabled = manController;
    }

    public void OneTimeCheck()
    {
        PerFrameLogic();
    }

    public bool GetFaceRight()
    {
        return faceRight;
    }
}

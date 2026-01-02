using System;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class WallCheck2 : MonoBehaviour
{
    public event Action<bool> OnWallCollisionChanged; // subscribed to by PlayerStateManager.DoStateGroundedChange(bool isGrounded)
    [SerializeField] bool IsInWall = false;
    [Tooltip("Used to filter which layers should be processed by TriggerEnter/Exit")]
    [SerializeField] LayerMask TargetLayerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayerMask) != 0)
        {
            ///print("entering: " + collision.name);
            IsInWall = true;
            OnWallCollisionChanged?.Invoke(IsInWall);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayerMask) != 0)
        {
            //print("exiting: " + collision.name);
            IsInWall = false;
            OnWallCollisionChanged?.Invoke(IsInWall);
        }
    }

    public bool GetIsInWall() => IsInWall;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager
{
    Vector2 respawnPoint;

    public void UpdateRespawnPoint(Vector3 newPosition)
    {
        respawnPoint = new Vector2(newPosition.x, newPosition.y);
    }
    
    // A fade called here in the RespawnManager will fade out, respawn, then fade in
    // call ScreenCover.cs directly if respawn is not desired
    public void ExecuteVoidOut()
    {
        // Fade to black (workaround becuase I can't use coroutines without monobehavior :(
        GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().FadeToBlackThenRespawn();
        // starts process that will eventually call VoidOutCompleted
    }

    public void FinishedFadeOut()
    {
        // move position to quickRespawnPoint
        GameMaster.GM.thePlayer.transform.position = respawnPoint;

        // fade back in now that player has been moved
        GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().FadeIn();
    }

}

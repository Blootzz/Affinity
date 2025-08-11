using UnityEngine;

public class EnemyHitboxManager : MonoBehaviour
{
    /// <summary>
    /// Used at beginning of every attack that uses colliders because colliders are disabled upon collision with player or shield
    /// </summary>
    public void EnableAllColliders()
    {
        //print("enabling all colliders");
        foreach (Transform child in transform)
        {
            child.GetComponent<Collider2D>().enabled = true;
        }
    }
}

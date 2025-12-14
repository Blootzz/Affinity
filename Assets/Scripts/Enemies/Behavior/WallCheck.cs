using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    string objectTag;
    GameObject stateManagerObject;

    // Start is called before the first frame update
    void Start()
    {
        stateManagerObject = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        objectTag = otherCollider.gameObject.tag;
        // I have no idea what the "object" is when importing maps from Tiled. Otherwise, I would find where that object is and change its tag to "Ground"
        if (otherCollider.gameObject.name.CompareTo("Collision_Default") == 0 || objectTag.CompareTo("Enemy")==0 || objectTag.CompareTo("Ground") == 0 || objectTag.CompareTo("Breakable Object") == 0 || objectTag.CompareTo("Unbreakable Object") == 0)
        {
            if (stateManagerObject.gameObject.CompareTag("Player"))
            {
                // logic if player uses this script
            }
            else
            {
                stateManagerObject.GetComponent<WalkBlind>().ActAtLedgeOrWall();
            }
        }// check to see if the other box collider is ground
    }

}

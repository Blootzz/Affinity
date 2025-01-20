using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCheck : MonoBehaviour
{
    string objectTag;
    GameObject myParent;

    // Start is called before the first frame update
    void Start()
    {
        myParent = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        objectTag = otherCollider.gameObject.tag;
        // I have no idea what the "object" is when importing maps from Tiled. Otherwise, I would find where that object is and change its tag to "Ground"
        if (otherCollider.gameObject.name.CompareTo("Collision_Default") == 0 || objectTag.CompareTo("Ground") == 0 || objectTag.CompareTo("Breakable Object") == 0 || objectTag.CompareTo("Unbreakable Object") == 0)
        {
            if (myParent.gameObject.CompareTag("Player"))
            {
                // logic if player uses this script
            }
            else
            {
                myParent.GetComponent<WalkBlind>().ActAtLedgeOrWall();
            }
        }// check to see if the other box collider is ground
    }
}

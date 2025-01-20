using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Grounded : MonoBehaviour
{
    GameObject myParent;
    string objectTag;
    public delegate void DelegateType(); //defines DelegateType as void with no parameters
    public event DelegateType OnGroundedEnter;
    public event Action OnGroundedExit; // Used in Double Jump

    // Start is called before the first frame update
    void Start()
    {
        myParent = gameObject.transform.parent.gameObject;

    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        objectTag = otherCollider.gameObject.tag;
        // I have no idea what the "object" is when importing maps from Tiled. Otherwise, I would find where that object is and change its tag to "Ground"
        if (otherCollider.gameObject.name.CompareTo("Collision_Default") == 0 || objectTag.CompareTo("Ground") == 0 || objectTag.CompareTo("Breakable Object") == 0 || objectTag.CompareTo("Unbreakable Object") == 0)
        {
            if (myParent.gameObject.CompareTag("Player"))
            {
                myParent.GetComponent<Player>().isGrounded = true;
                myParent.GetComponent<Player>().isFalling = false;
                myParent.GetComponent<Player>().ledgeGrabActive = false;
                myParent.GetComponent<Player>().CreateLandingDust();
                myParent.GetComponent<Player>().DisableWallJumpBox();
            }
            else
            {
                myParent.GetComponent<Enemy>().isGrounded = true;
                if (myParent.GetComponent<Enemy>().stunned == false)
                    myParent.GetComponent<Enemy>().Land();
            }
        }// check to see if the other box collider is ground
        if (OnGroundedEnter != null)
            OnGroundedEnter();
    }

    //public void TriggerEnterEvent()
    //{
    //    // This method is assigned to event listeners in classes like DoubleJump
    //    // Some logic is done in OnTriggerEnter2D before this was created
    //}// Event

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        objectTag = otherCollider.gameObject.tag;
        if (otherCollider.gameObject.name == "Collision_Default" || objectTag.CompareTo("Ground") == 0 || objectTag.CompareTo("Breakable Object") == 0 || objectTag.CompareTo("Unbreakable Object") == 0 || otherCollider.gameObject.name == "Dummy")
        {
            if (myParent.gameObject.CompareTag("Player"))
            {
                Player thisPlayer = myParent.GetComponent<Player>();
                thisPlayer.isGrounded = false;
                if (!thisPlayer.controlsDisabled && !thisPlayer.attacking) // prevents triggering fall animation when taking knockback
                {
                    thisPlayer.animator.Play(thisPlayer.AorUFalling);
                }
            }
            else
                myParent.GetComponent<Enemy>().isGrounded = false;
        }// check to see if the other box collider is ground
        if (OnGroundedExit != null)
            OnGroundedExit();
    }

    //public void TriggerExitEvent()
    //{
    //    // to be used as a listener in other classes
    //}
}

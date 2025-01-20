using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox1Script : MonoBehaviour
{
    Player thePlayer;
    string objectTag;
    List<int> naughtyList = new List<int>(); // List of enemies that have been hit by hitbox. Used to prevent double hits and allotting gifts

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // prevent same object from 
        if (naughtyList.Contains(collision.gameObject.GetInstanceID()))
            return;
        naughtyList.Add(collision.gameObject.GetInstanceID());
        // naughtyList must be cleared when the hitbox deactivates

        objectTag = collision.gameObject.tag;
        if (objectTag.Equals("Enemy") || objectTag.Equals("Breakable Object"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(thePlayer.attackDamage, thePlayer.attackStunTime, thePlayer.attackKnockbackMultiplier, thePlayer.attackKnockbackAngle);
            thePlayer.attackConnected = true; // tell the player they landed a hit
            // hit effects
        }
    }

    public void ClearNaughtyList() //TO DO: CALL THIS IN PLAYER._FINISHATTACK()
    {
        naughtyList.Clear();
        naughtyList.TrimExcess();
    }// sets list to straight zeros
}
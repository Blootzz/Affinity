using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbilitySelection : MonoBehaviour
{
    Shield theShield; // shorthand for gameobject.getComponent<Sheild>()
    int numAbilities;

    void Start()
    {
        theShield = gameObject.GetComponent<Shield>();
        numAbilities = theShield.transform.childCount; // number of children in shield (children in shield object should only be abilities)
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GameMaster.GM.controlManager.cycleRightKey))
            IncrementAbilityUp();
        if (Input.GetKeyDown(GameMaster.GM.controlManager.cycleLeftKey))
            IncrementAbilityDown();
    }

    void IncrementAbilityUp()
    {
        // comparing current index vs max (total number of shield abilities)
        if (gameObject.GetComponent<Shield>().selectedAbilityIndex < numAbilities-1) // if index has room to increment up
            gameObject.GetComponent<Shield>().selectedAbilityIndex += 1;
        else
            gameObject.GetComponent<Shield>().selectedAbilityIndex = 0;   // cycle back to 0 if index is at numAbilities-1
    }

    void IncrementAbilityDown()
    {
        if (gameObject.GetComponent<Shield>().selectedAbilityIndex > 0) // if index has room to go down
            gameObject.GetComponent<Shield>().selectedAbilityIndex -= 1; // move down
        else
            gameObject.GetComponent<Shield>().selectedAbilityIndex = numAbilities - 1; // if index is already equal to 0, bring it back to highest index

    }

}

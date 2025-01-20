using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linse : Speaker
{
    public override void BeginDialogue()
    {
        base.BeginDialogue();
        StopAllCoroutines();
        //StopCoroutine(FlipRandomly());
        watchPlayer = true;
    }

    public override void ResumeLife() // does nothing by default
    {
        base.ResumeLife();
        watchPlayer = false;
        StartCoroutine(FlipRandomly());
    }

    IEnumerator FlipRandomly()
    {
        if (!watchPlayer)
        {
            yield return new WaitForSeconds(Random.Range(0.4f, 4f));
            Flip();
            StartCoroutine(FlipRandomly());
        }
    }

    // ============================ ACTIONS CALLED IN DIALOGUE GUI ============================
    public override void ExecuteThisAction(string typedParameter) // \amethod\ in dialogue GUI, where <method> is the string name of a method
    {
        Invoke(typedParameter, 0f); // calls on next frame
    }
    //---------------------- METHODS (must be lowercase) --------------------------
    void test()
    {
        print("Invoke successful for Linse");
    }

}

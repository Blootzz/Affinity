using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeDoor : Interactable
{
    public override void Execute()
    {
        base.Execute();
        GetComponent<Animator>().SetTrigger("OpenDoor");
    }

    public void LoadNewArea()
    {
        GetComponent<AreaLoader>().LoadScene();
    }

}

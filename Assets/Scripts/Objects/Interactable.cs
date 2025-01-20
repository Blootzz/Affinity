using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isAvailableToInteract = true;

    public virtual void Execute()
    {
        isAvailableToInteract = false;
    }
}

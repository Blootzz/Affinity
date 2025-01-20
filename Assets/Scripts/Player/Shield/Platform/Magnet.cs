using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    int Magnetize;

    private void Awake()
    {
        Magnetize = Animator.StringToHash("Magnetize");
    }

    private void OnEnable()
    {
        transform.parent.GetComponent<Animator>().Play(Magnetize);
    }

}

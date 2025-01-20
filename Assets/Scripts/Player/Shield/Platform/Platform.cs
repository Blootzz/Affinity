using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Shield theShield;
    int plat;

    private void Awake()
    {
        theShield = transform.parent.GetComponent<Shield>();
        plat = Animator.StringToHash("ShieldPlatform");
    }

    private void OnEnable()
    {
        theShield.isPlatform = true;
        theShield.GetComponent<Animator>().Play(plat);
    }

    private void OnDisable()
    {
        theShield.isPlatform = false;
    }
}

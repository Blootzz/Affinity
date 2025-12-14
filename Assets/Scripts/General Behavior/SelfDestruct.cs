using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [Tooltip("Option to enable Destroy based off time from Start()")]
    [SerializeField] bool useStartTimer = true;
    public float time = 2f;

    // Start is called before the first frame update
    void Start()
    {
        if (useStartTimer)
            Destroy(gameObject, time);
    }

    public void ANIM_DestroyByAnimation()
    {
        Destroy(gameObject);
    }
}

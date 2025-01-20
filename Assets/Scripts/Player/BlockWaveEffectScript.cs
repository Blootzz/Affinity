using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWaveEffectScript : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Animator>().Play("BlockWaveEffect");
    }

    void _SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}

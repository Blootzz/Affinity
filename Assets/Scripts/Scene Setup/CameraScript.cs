using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Player thePlayer;

    private void Start()
    {
        thePlayer = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y, -10);
    }
}

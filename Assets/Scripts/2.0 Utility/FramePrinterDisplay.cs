using UnityEngine;
using System;

public class FramePrinterDisplay : MonoBehaviour
{
    float frameCount = 0;

    private void Update()
    {
        print("============== Frame: " + frameCount);
        if (++frameCount > 100000)
            frameCount = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrummingArmSpriteSelection : MonoBehaviour
{
    public Sprite[] sprites;
    public int armIndex = 0;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[armIndex];
    }

    public void Strum() // swaps strumming sprites (there are only two)
    {
        if (armIndex == 0)
            armIndex++;
        else
            armIndex--;

        sr.sprite = sprites[armIndex];
    }

}

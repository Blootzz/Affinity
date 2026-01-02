using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarSpriteSelection : MonoBehaviour
{
    public Sprite[] sprites;
    public int guitarIndex = 0;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[guitarIndex];
    }

    public void CycleGuitar(bool forwards)
    {
        if (forwards)
            guitarIndex++;
        else
            guitarIndex--;

        CatchOutOfBounds();

        sr.sprite = sprites[guitarIndex];
    }

    void CatchOutOfBounds()
    {
        if (guitarIndex < 0)
            guitarIndex = sprites.Length - 1;
        if (guitarIndex >= sprites.Length)
            guitarIndex = 0;
    }
}

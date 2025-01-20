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

    public void ChangeGuitar(int newIndex)
    {
        guitarIndex = newIndex;
        sr.sprite = sprites[guitarIndex];
    }

}

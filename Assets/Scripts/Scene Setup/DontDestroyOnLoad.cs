using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // USE PREVENT DUPLICATION SCRIPT IF APPLICABLE
    // THIS WILL RESULT IN DUPLICATION IF LOADING A SCENE THAT ALREADY HAS THIS
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}

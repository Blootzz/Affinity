using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBoss : MonoBehaviour
{
    Animator animator;
    public Player thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        thePlayer = GameMaster.GM.thePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

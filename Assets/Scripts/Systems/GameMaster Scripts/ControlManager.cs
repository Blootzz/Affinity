using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager
{
    // Basics
    public KeyCode testKey = KeyCode.T;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode ledgeGrabKey = KeyCode.E;
    public KeyCode attackKey = KeyCode.Mouse0;

    // Shield Stuff
    public KeyCode blockKey = KeyCode.B;
    public KeyCode spellMenuKey = KeyCode.Q;
    public KeyCode throwKey = KeyCode.Mouse1; // throw is also parry
    public KeyCode throwKey2 = KeyCode.J;
    public KeyCode cycleRightKey = KeyCode.E; // cycle shield ability (shield, magnet, etc)
    public KeyCode cycleLeftKey = KeyCode.Q; // cycle shield ability

    // menus
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode menuUp = KeyCode.W;
    public KeyCode menuDown = KeyCode.S;
    public KeyCode menuLeft = KeyCode.A;
    public KeyCode menuRight = KeyCode.D;
    public KeyCode menuAdvance = KeyCode.Space;

    //public enum KeyBinds
    //{
    //    // basics
    //    testKey = KeyCode.T,
    //    upKey = KeyCode.W,
    //    downKey = KeyCode.D,
    //    leftKey = KeyCode.A,
    //    rightKey = KeyCode.D,

    //    // general
    //    pauseKey = KeyCode.Escape,

    //    // player control
    //    jumpKey = KeyCode.Space,
    //    ledgeGrabKey = KeyCode.E,
    //    blockKey = KeyCode.B,
    //    spellMenuKey = KeyCode.Q,
    //    throwKey = KeyCode.Mouse1, // throw is also parry
    //    throwKey2 = KeyCode.J,
    //    attackKey = KeyCode.Mouse0

    //}// keybinds

}

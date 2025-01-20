using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCover : MonoBehaviour
{
    // ScreenCover has a flat rectangular image that covers the screen
    // Its alpha is 0 until a FadeTo____() function is called

    Image screenImage;
    float fadeSpeed = 0.04f;    // amount alpha changes by on every frame
    float loadTime = 1;         // time the screen will be completedly covered
    public bool isScreenCovered = false; // used in while loops to create flagging system to tell RespawnManager, AreaLoader, etc. when fade is complete

    private void Start()
    {
        screenImage = GetComponent<Image>();
    }

    // call ScreenCover.cs methods directly if trying to fade to black without respawning
    // call RespawnManager.cs methods to respawn (which will call these fade methods)

    // USE THE FOLLOWING CODE TO DETERMINE WHETHER IF THE SCREEN HAS COMPLETED A FADE OUT
    // IEnumerator Wait()
    // {
    //        while(GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().isScreenCovered == false)
    //        {
    //            yield return new WaitForEndOfFrame();
    //        }// proceed once isScreenCovered == true
    //          <DO STUFF>
    //   }

    public void FadeToBlack()
    {
        StartCoroutine(FadeTo(0, false)); // (0h, 0s, 0 Value)
    }
    
    public void FadeToBlackThenRespawn()// workaround because RespawnManager can't use coroutines due to not inhereting monobehavior
    {
        StartCoroutine(FadeTo(0, true));
    }

    public void FadeToWhite()
    {
        StartCoroutine(FadeTo(1, false)); // (0h, 0s, 1 Value)
    }

    IEnumerator FadeTo(int value, bool respawn)
    {
        Color tempColor = Color.HSVToRGB(0, 0, value);
        screenImage.color = tempColor;
        // sets to black or white, depending on value
        // tempColor is used in the for loop to be the color that gets manipulated. screenImage.color cannot be tampered with unless it is set to a new color entirely

        for (float a = 0; a < 1; a += fadeSpeed)
        {
            tempColor.a = a;
            screenImage.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        // clean up error
        tempColor.a = 1;
        screenImage.color = tempColor;

        isScreenCovered = true;

        if (respawn) // determines whether or not to activate respawn sequence
            GameMaster.GM.respawnManager.FinishedFadeOut();

        //yield return new WaitForSecondsRealtime(loadTime);
        //FadeIn();
    }

    /// Note about Color
    /// Color(r,g,b,a) or Color(r,g,b) use values of 0-1
    /// Color32(r,g,b,a) or Color32(r,g,b) use values 0-255
    
    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        // value does not need to be adjusted
        Color tempColor = screenImage.color;

        for (float a = 1; a > 0; a -= fadeSpeed)
        {
            tempColor.a = a;
            screenImage.color = tempColor;
            yield return new WaitForEndOfFrame();
        }
        // clean up error
        tempColor.a = 0;
        screenImage.color = tempColor;
        isScreenCovered = false;
    }

}

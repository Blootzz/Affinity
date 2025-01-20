using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaLoader : MonoBehaviour
{
    public int iLevelToLoad;
    public string sLevelToLoad;

    public int positionInNewScene; // 0 indexed
    public bool useIntegerToLoadLevel = false;
    LoadPointFinder playerLoadPointFinder; // Attached to player

    // Start is called before the first frame update
    void Start()
    {
        //playerLoadPointFinder = FindObjectOfType<LoadPointFinder>();
        playerLoadPointFinder = GameMaster.GM.thePlayer.GetComponent<LoadPointFinder>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        // do not use RespawnManager because we want to avoid placing the player somewhere besides the positionInNewScene location
        GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().FadeToBlack();
        StartCoroutine(WaitAndLoad());

    }// LoadScene()

    IEnumerator WaitAndLoad()
    {
        while(GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().isScreenCovered == false)
        {
            yield return new WaitForEndOfFrame();
        }// proceed once isScreenCovered == true
        Load();
    }

    void Load()
    {
        if (useIntegerToLoadLevel) // use integer to load scene number
        {
            playerLoadPointFinder.UpdateTargetNumber(positionInNewScene);

            // LoadPointFinder.cs has a listener for this Event so that it can execute code AFTER scene has changed
            SceneManager.LoadSceneAsync(iLevelToLoad);
            // code here does not have access to new scene
        }
        else // use string name to load
        {
            playerLoadPointFinder.UpdateTargetNumber(positionInNewScene);

            // LoadPointFinder.cs has a listener for this Event so that it can execute code AFTER scene has changed
            SceneManager.LoadSceneAsync(sLevelToLoad);
            // code here does not have access to new scene
        }

        GameMaster.GM.mainCanvas.GetComponentInChildren<ScreenCover>().FadeIn();
    }
}

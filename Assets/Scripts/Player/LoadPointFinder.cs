using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPointFinder : MonoBehaviour
{
    public LoadPoint targetedPoint;
    int targetNumber;// number sent by AreaLoader.positionInNewScene

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // adds OnSceneLoaded to sceneLoaded. OnSceneLoaded() is not called until sceneLoaded is
    }

    public void UpdateTargetNumber(int target)
    {
        targetNumber = target;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) // must have this signature
    {
        // Called on Unity Event sceneLoaded
        // AreaLoader.cs will update targetNumber before loading scene
        CountAndMoveToLoadPoint(targetNumber);
    }

    public void CountAndMoveToLoadPoint(int pos) // called after succesful scene load
    {
        LoadPoint[] loadPoints = FindObjectsOfType<LoadPoint>();
        foreach (LoadPoint thisPoint in loadPoints)
        {
            if (thisPoint.positionNumber == pos)
            {
                targetedPoint = thisPoint;
                MovePlayerToTargetedPoint();
            }// targetPoint updated and moved to
        }// for each loop in loadPoints[]
    }

    void MovePlayerToTargetedPoint()
    {
        gameObject.transform.position = targetedPoint.transform.position;
    }


}

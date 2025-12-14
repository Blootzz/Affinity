using UnityEngine;
using System;
using Unity.Cinemachine;
using System.Collections;

public class LensZoomEffect : MonoBehaviour
{
    // animation will determin percent zoom starting at 1.0 and go 
    [Tooltip("The animated ratio the camera should zoom upon a parry. 0.5 = zoomed in halfway, 2.0 = zoomed out double")]
    public float animatedZoomRatio = 1;
    float startingLensOrthoSize;

    CinemachineCamera cineCam;
    Animator animator;

    [InspectorButton(nameof(OnButtonClicked))]
    public bool zoom;
    private void OnButtonClicked() {
        GetComponent<Animator>().Play("Parry Zoom", -1, 0);
    }

    private void Start()
    {
        cineCam = GetComponent<CinemachineCamera>();
        animator = GetComponent<Animator>();
        startingLensOrthoSize = cineCam.Lens.OrthographicSize;
    }

    public void BeginParryZoomAnimation()
    {
        animator.Play("Parry Zoom", -1, 0);
    }

    private void Update()
    {
        cineCam.Lens.OrthographicSize = startingLensOrthoSize * animatedZoomRatio;
    }
}

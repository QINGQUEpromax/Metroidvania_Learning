using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPosition;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;


    private void Awake()
    {
        mainCamera = Camera.main;  
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        InitializeLayers();
    }

    private void FixedUpdate()
    {
        float currentCameraPosition = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPosition - lastCameraPosition; 
        lastCameraPosition = currentCameraPosition;

        float cameraLeftEdge = currentCameraPosition - cameraHalfWidth;
        float cameraRightEdge = currentCameraPosition + cameraHalfWidth;

        foreach (var layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach(var image in backgroundLayers)
        {
            image.CalculateImageWidth();
        }
    }
}

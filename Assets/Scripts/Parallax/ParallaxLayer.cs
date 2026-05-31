using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class ParallaxLayer 
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;

    private float imageFullWidth;
    private float imageHalfWidth;


    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;

    }

    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (parallaxMultiplier * distanceToMove);
    }

    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)
    {
        float imageLeftEdge = background.position.x - imageHalfWidth;
        float imageRightEdge = background.position.x + imageHalfWidth;

        if (imageLeftEdge >= cameraRightEdge)
        {
            background.position -= Vector3.right * imageFullWidth;
        }
        else if(imageRightEdge <= cameraLeftEdge)
        {
            background.position += Vector3.right * imageFullWidth;
        }

    }
}

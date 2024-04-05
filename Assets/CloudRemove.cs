using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudRemove : MonoBehaviour
{
    private Camera mainCamera;
    private float lowerBound;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        // Calculate the lower bound relative to the camera position and height
        lowerBound = mainCamera.transform.position.y - mainCamera.orthographicSize - 1; // Additional buffer of 1 unit
    }

    void Update()
    {
        // Check if the cloud is below the lower boundary
        if (transform.position.y < lowerBound)
        {
            Destroy(gameObject); // Destroy the cloud
        }
    }
}

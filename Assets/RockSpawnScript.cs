using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnScript : MonoBehaviour
{
    public GameObject[] Rocks; // Array of rock prefabs to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public float minimumDistance = 5f; // Minimum distance between consecutive rocks
    private float timer; // Timer to track spawn timing
    private Vector3 lastSpawnPosition; // To track the position of the last spawned rock
    private bool isFirstRock = true; // To check if it's the first rock being spawned

    void Update()
    {
        // Increment the timer by the elapsed time since last frame
        timer += Time.deltaTime;

        // Check if the timer exceeds the specified spawn interval
        if (timer > spawnInterval)
        {
            // Spawn a rock and reset the timer
            SpawnRock();
            timer = 0;
        }
    }

    void SpawnRock()
    {
        // Randomly pick a rock prefab from the array
        GameObject rockToSpawn = Rocks[Random.Range(0, Rocks.Length)];

        // Calculate a random x position within a specified range from the spawner's position
        float randomX;

        if (isFirstRock)
        {
            randomX = transform.position.x + Random.Range(-10f, 10f);
            isFirstRock = false;
        }
        else
        {
            do
            {
                randomX = transform.position.x + Random.Range(-10f, 10f);
            } 
            while (Mathf.Abs(randomX - lastSpawnPosition.x) < minimumDistance);
        }

        Vector3 spawnPosition = new Vector3(randomX, transform.position.y, 0);

        // Instantiate the selected rock prefab at the calculated position
        Instantiate(rockToSpawn, spawnPosition, Quaternion.identity);

        // Update the last spawn position
        lastSpawnPosition = spawnPosition;
    }
}

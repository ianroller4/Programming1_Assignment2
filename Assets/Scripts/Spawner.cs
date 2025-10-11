using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // --- Asteroid game object reference
    public GameObject asteroidPreFab;

    // --- Impulse Variables ---
    public int lowSpawnImpulse = 250;
    public int highSpawnImpulse = 500;

    // --- Spawn Location Variables ---
    public float spawnDistance = 20f;

    // --- Switches ---
    public bool DEBUG_MODE = false;

    // --- Spawn Rate Variables ---
    public float spawnTimeMax = 5f;
    private float spawnTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!DEBUG_MODE)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTimeMax)
            {
                Spawn();
                spawnTimer -= spawnTimeMax;
            }
        }
        else
        {
            //if (Input.GetKeyDown("space"))
            //{
            //    Spawn();
            //}
        }
    }

    private void Spawn()
    {
        if (asteroidPreFab != null)
        {
            // Create instance of asteroid prefab
            GameObject asteroid = Instantiate(asteroidPreFab);

            // Check for rigidbody
            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Set position randomly around the arena
                Vector2 spawnDirection = Random.insideUnitCircle;
                Vector2 spawnPosition = spawnDirection * spawnDistance;
                asteroid.transform.position = spawnPosition;
                // Set trajectory 
                Vector2 trajectory = -spawnDirection;
                // Add impulse
                int impulse = Random.Range(lowSpawnImpulse, highSpawnImpulse);
                if (DEBUG_MODE)
                {
                    Debug.Log(impulse);
                }
                rb.AddForce(trajectory * impulse, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning("No RigidBody2D on asteroid instance.");
            }
        }
        else
        {
            Debug.LogWarning("No Asteroid prefab attached");
        }
    }
}

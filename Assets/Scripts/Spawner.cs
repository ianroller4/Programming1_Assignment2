using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Spawner
 * 
 * Spawns asteroids into the arena at set intervals
 * 
 */
public class Spawner : MonoBehaviour
{
    // --- Asteroid Pre Fab Reference ---
    public GameObject asteroidPreFab;

    // --- Impulse Variables ---
    public int lowSpawnImpulse = 250;
    public int highSpawnImpulse = 500;

    // --- Spawn Location Variables ---
    public float spawnDistance = 20f;

    // --- Switches ---
    public bool DEBUG_MODE = false; // Used to enable or disable debug options

    // --- Spawn Rate Variables ---
    public float spawnTimeMax = 5f; // Length of time between spawns
    private float spawnTimer = 0f;

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Start()
    {
        if (!DEBUG_MODE)
        {
            Spawn(); // Spawn asteroid at very start
        }
        else
        {
            // Create instance of asteroid prefab
            GameObject asteroid = Instantiate(asteroidPreFab);
            asteroid.transform.position = new Vector3(0, 3, 1);
        }
    }

    /* Update
     * 
     * Called once per frame
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Update()
    {
        if (!DEBUG_MODE)
        {
            SpawnTimer(); // Update spawn timer
        }
    }

    /* SpawnTimer
     * 
     * Updates the spawn timer and calls Spawn on timeout
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void SpawnTimer()
    {
        // Update timer
        spawnTimer += Time.deltaTime;

        // Check if max time reached or exceded
        if (spawnTimer >= spawnTimeMax)
        {
            // Spawn asteroid
            Spawn();

            // Set spawn timer back the max time between spawns to prevent spawn duration extension
            spawnTimer -= spawnTimeMax;
        }
    }

    /* Spawn
     * 
     * Spawns an asteroid and adds initial forces
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Spawn()
    {
        // Check that the pre fab is present
        if (asteroidPreFab != null)
        {
            // Create instance of asteroid prefab
            GameObject asteroid = Instantiate(asteroidPreFab);

            // Get rigidbody component
            Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();

            // Check that the rigidbody is not null
            if (rb != null)
            {
                // Set position randomly around the arena
                Vector2 spawnDirection = Random.insideUnitCircle; // Gets position within / on a unit circle
                Vector2 spawnPosition = spawnDirection * spawnDistance; // Extends the position to spawn distance
                asteroid.transform.position = spawnPosition; // Set the asteroids position to the newly created position

                // Set trajectory 
                Vector2 trajectory = -spawnDirection; // Sets direction of asteroid towards the center of the arena

                // Add impulse
                int impulse = Random.Range(lowSpawnImpulse, highSpawnImpulse); // Get impulse value randomly between low and high impulse variables
                rb.AddForce(trajectory * impulse, ForceMode2D.Impulse); // Impulse force in the direction of impulse and with the strength of impulse

                if (DEBUG_MODE)
                {
                    Debug.Log(impulse);
                }
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

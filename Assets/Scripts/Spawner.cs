using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // --- Asteroid game object reference
    public GameObject asteroidPreFab;

    // --- Impulse Variables ---
    public float lowSpawnImpulse = 500f;
    public float highSpawnImpulse = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Spawn();
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
                float xPosition = Random.Range(-7f, 7f);
                float yPosition = Random.Range(-4f, 4f);
                asteroid.transform.position = new Vector3(0f, 3f, 0f);
                // Set tracjectory 
                Vector2 trajectory = new Vector2(-1, 0);
                // Add impulse
                rb.AddForce(trajectory * 100, ForceMode2D.Impulse);
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

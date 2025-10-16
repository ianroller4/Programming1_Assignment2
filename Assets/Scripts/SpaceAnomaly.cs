using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Space Anomaly
 * 
 * Space anomaly simulates a random object in space that affects
 * the player without damaging them when they get too close.
 * 
 */
public class SpaceAnomaly : MonoBehaviour
{
    // --- Component References ---
    Rigidbody2D rb; // Rigidbody of the space anomaly

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Start()
    {
        // Get rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Add forces to rigidbody to get the space anomaly moving
        rb.AddTorque(Random.Range(50, 100)); // Random torgue added between 50 and 100
        Vector2 forceDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)); // Random movement direction
        rb.AddForce(forceDirection * Random.Range(50, 100), ForceMode2D.Impulse); // Add random impulse force in direction of movement 
    }

    /* OnTriggerEnter2D
     * 
     * Called when another object enters its collision shape
     * 
     * Parameters: Collider2D collision, the collider that entered the collision shape
     * 
     * Return: None
     * 
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Get game object of collision
        GameObject collidedWith = collision.gameObject;

        // Check that a game object exists
        if (collidedWith != null)
        {
            // Check the game object is on the player layer
            if (collidedWith.layer == 6)
            {
                // Check that the game object has a rigidbody
                if (collidedWith.GetComponent<Rigidbody2D>() != null)
                {
                    // Get rigidbody and apply forces 
                    Rigidbody2D rbCollided = collidedWith.GetComponent<Rigidbody2D>();
                    rbCollided.AddTorque(Random.Range(50, 100)); // Random torgue added between 50 and 100
                    Vector2 forceDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)); // Random movement direction
                    rbCollided.AddForce(forceDirection * Random.Range(50, 100)); // Add random impulse force in direction of movement 
                }
                else
                {
                    Debug.LogWarning("No rigidbody found on game object.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No game object exists with the collider.");
        }
    }
}

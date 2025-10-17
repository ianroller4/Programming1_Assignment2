using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ExtraLife
 * 
 * Special item that randomly spawns to heal the player +1 health
 * 
 */
public class ExtraLife : MonoBehaviour
{
    // --- Heal Amount ---
    private float healAmount = 1f;

    // --- Switches ---
    public bool DEBUG_MODE = false; // Used to enable or disable debug options

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
                if (DEBUG_MODE)
                {
                    Debug.DrawLine(transform.position, collidedWith.transform.position, Color.green);
                }
                else
                {
                    // Check that the game object has a health component
                    if (collidedWith.GetComponent<Health>() != null)
                    {
                        collidedWith.GetComponent<Health>().Heal(healAmount);
                        Destroy(gameObject);
                    }
                    else
                    {
                        Debug.LogWarning("No health found on game object.");
                    }
                }
                
            }
            else
            {
                if (DEBUG_MODE)
                {
                    Debug.DrawLine(transform.position, collidedWith.transform.position, Color.red);
                }
            }
        }
        else
        {
            Debug.LogWarning("No game object exists with the collider.");
        }
    }
}

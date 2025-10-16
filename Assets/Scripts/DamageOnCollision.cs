using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* DamageOnCollision
 * 
 * Handles collisions between asteroids and the player dealing damage to the player
 * 
 */

// Require Rigidbody2D and Collider2D
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DamageOnCollision : MonoBehaviour
{
    // --- Damage Variables ---
    private float damageToDeal = 1f;

    // --- Explosion Pre Fab ---
    public GameObject explosion;

    /* OnCollisionEnter2D
     * 
     * Called when another objects collision shape collides with this objects collision shape
     * 
     * Parameters: Collider2D collision, the collider that collided with the collision shape
     * 
     * Return: None
     * 
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Get game object of collider
        GameObject collidedWith = collision.gameObject;

        // Check that there is a game object
        if (collidedWith != null)
        {
            // Check the game object is on the player layer
            if (collidedWith.layer == 6)
            {
                // Make sure the game object has a health script component
                if (collidedWith.GetComponent<Health>() != null)
                {
                    // Check that the explosion pre fab is not null
                    if (explosion != null)
                    {
                        // Create instance of explosion pre fab
                        GameObject explode = Instantiate(explosion);

                        // Change position of explosion prefab to point of contact
                        explode.transform.position = collision.GetContact(0).point;

                        // Change scale of explosion prefab to a more reasonable size
                        explode.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                    }
                    // Get health component and deal damage
                    collidedWith.GetComponent<Health>().TakeDamage(damageToDeal);
                }
                else
                {
                    Debug.LogWarning("No health component found on game object.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No game object exists with the collider.");
        }
    }
}

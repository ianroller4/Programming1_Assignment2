using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BoundaryWarp
 * 
 * Creates a wrapping effect so that objects do not go off the screen
 * 
 */

// Require Rigidbody2D component
[RequireComponent(typeof(Rigidbody2D))]
public class BoundaryWarp : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;

    // --- Camera Size Variables ---
    private Vector2 topRightScreen;
    private Vector2 bottomLeftScreen;

    // --- Position of Game Object ---
    private Vector3 positionOnScreen;

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
        // Get rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Set up Camera size variables
        topRightScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)); // Top right corner for top and righ checks
        bottomLeftScreen = Camera.main.ScreenToWorldPoint(Vector2.zero); // Bottom left corner for bottom and left checks
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
        UpdatePosition();

        BoundaryWrap();  
    }

    /* Update Position
     * 
     * Updates the position on screen of the game object with respect to the camera
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void UpdatePosition()
    {
        // Get position of object in the screen in pixels
        positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
    }

    /* BoundaryWrap
     * 
     * Changes the position of the player if they cross a boundary 
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void BoundaryWrap()
    {
        // If passing left side of screen and moving left
        if (positionOnScreen.x <= 0 && rb.velocity.x < 0)
        {
            transform.position = new Vector2(topRightScreen.x, transform.position.y);
        }
        // If passing right side of screen and moving right
        else if (positionOnScreen.x >= Screen.width && rb.velocity.x > 0)
        {
            transform.position = new Vector2(bottomLeftScreen.x, transform.position.y);
        }
        // If passing bottom side of screen and moving down
        else if (positionOnScreen.y <= 0 && rb.velocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, topRightScreen.y);
        }
        // If passing top side of screen and moving up
        else if (positionOnScreen.y >= Screen.height && rb.velocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, bottomLeftScreen.y);
        }
    }
}

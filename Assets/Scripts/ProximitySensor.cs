using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Proximity Sensor
 * 
 * Detects and warns the player when enemy objects are close to the player.
 * A trigger is used to accomplish this.
 * 
 */
public class ProximitySensor : MonoBehaviour
{
    // --- Proximity Alert Variables ---
    private SpriteRenderer redRing; // A red ring that surrounds the player to warn that something is too close
    private float alertTimer = 0; // The timer for how long the red ring stays active
    private bool alertTimerStarted = false; // Whether or not the timer has started
    public float ALERT_TIME_MAX = 1f; // How long the timer is

    // --- List of Dangers ---
    private List<GameObject> dangers; // List of dangers close by. Used to draw lines to all dangers in debug mode

    // --- Switches ---
    public bool DEBUG_MODE = false; // Used to enable or disable debug options

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
        // Initialize list for dangers
        dangers = new List<GameObject>();

        // Get and set up sprite renderer
        redRing = GetComponent<SpriteRenderer>(); // Get the sprite renderer component
        redRing.enabled = false; // Disable the sprite renderer so the ring does not show
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
    private void Update()
    {
        // Update alert timer
        AlertTimer();

        if (DEBUG_MODE)
        {
            DrawDangerLines(); // Draw lines between player and any dangerous object in range
        }
    }

    /* AlertTimer
     * 
     * Handles timer functionality for turning the alert red ring off
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void AlertTimer()
    {
        // Check that timer is started
        if (alertTimerStarted)
        {
            // Update timer
            alertTimer += Time.deltaTime;
            // If timer has reached or surpassed max
            if (alertTimer >= ALERT_TIME_MAX)
            {
                // Disable red ring alert sprite
                redRing.enabled = false;

                // Reset timer
                alertTimer = 0;
                alertTimerStarted = false;
            }
        }
    }

    /* DrawDangerLines
     * 
     * Loops through danger list game objects and draws debug lines between player and objects
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void DrawDangerLines()
    {
        // Current position of player
        Vector3 position = transform.position;

        // Loop through dangers
        for (int i = 0; i < dangers.Count; i++)
        {
            // Draw line from player to object
            Debug.DrawLine(position, dangers[i].transform.position, Color.red);
        }
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
        // Get game object of collider
        GameObject collided = collision.gameObject;

        // Check that there is a game object
        if (collided != null)
        {
            // Check that the gameobject is on the correct layer
            if (collided.layer == 10)
            {
                // Add to danger list
                dangers.Add(collided);

                // Enable red ring alert and start timer
                alertTimerStarted = true;
                redRing.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("No game object exists with the collider.");
        }
    }

    /* OnTriggerExit2D
     * 
     * Called when another object exits its collision shape
     * 
     * Parameters: Collider2D collision, the collider that entered the collision shape
     * 
     * Return: None
     * 
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Get game object of collider
        GameObject collided = collision.gameObject;

        // Check that there is a game object
        if (collided != null)
        {
            // Check that the gameobject is on the correct layer
            if (collided.layer == 10)
            {
                // Check that the dangers list contains the object
                if (dangers.Contains(collided))
                {
                    // Remove object from dangers list
                    dangers.Remove(collided);
                }
                else
                {
                    Debug.LogWarning("The danger is not in the dangers list.");
                }
            }
        }
        else
        {
            Debug.LogWarning("No game object exists with the collider.");
        }
    }
}

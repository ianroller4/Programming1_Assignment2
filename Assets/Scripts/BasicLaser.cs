using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/* BasicLaser
 * 
 * Fires a laser to destroy asteroids. Uses a raycast to check for hits
 * 
 */
public class BasicLaser : MonoBehaviour
{
    // --- Fire Timer Variables ---
    public float FIRE_TIME = 0.25f;
    private float fireTimer = 0;

    // --- Cooldown Timer Variables --- 
    public float COOLDOWN_TIME = 1f;
    private float cooldownTimer = 0;

    // --- Raycast Variables ---
    public float distanceToFire = 6f; // Distance of raycast / laser
    public LayerMask layerMask; // Layer mask to determine which layers to check
    private RaycastHit2D hit; // Used to record raycast information

    // --- Hits ---
    private int objectsHit = 0; // Amount of objects hit during firing 

    // --- State Enum ---
    private enum States {
        READY,   // Will fire when button pressed 
        ACTIVE,  // Is firing, cannot fire again
        COOLDOWN // Is not firing, cannot fire yet
    };
    States currentState;

    // --- Line Renderer ---
    private LineRenderer lineRenderer;
    private Vector3 startPosition = new Vector3(0, 0.5f, -1); // Where to start drawing line

    // --- Switches ---
    public bool DEBUG_MODE = false; // Used to enable or disable debug options

    // --- Sound Effects ---
    public AudioSource audioSource;

    // --- Explosion Pre Fab ---
    public GameObject explosion;

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
        currentState = States.READY; // Initialize current state to ready
        lineRenderer = GetComponent<LineRenderer>(); // Get line renderer component
        SetUpLineRenderer(); // Set up line renderer values
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
        // Update the state machine
        StateMachine();
    }

    /* StateMachine
     * 
     * Handles running and switching between states
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void StateMachine()
    {
        // READY State
        if (currentState == States.READY)
        {
            // Check if space button pressed
            if (Input.GetButtonDown("Jump"))
            {
                // Fire Laser
                Fire();

                // Play fire sound
                PlaySound();

                // Switch to ACTIVE state
                currentState = States.ACTIVE;
            }
        }
        // ACTIVE State
        else if (currentState == States.ACTIVE)
        {
            // Update fire timer
            FireTimer();
        }
        // COOLDOWN State
        else if (currentState == States.COOLDOWN)
        {
            // Update cooldown timer
            CooldownTimer();
        }
    }

    /* FireTimer
     * 
     * Updates the fire timer and handles timeouts
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void FireTimer()
    {
        // Update timer
        fireTimer += Time.deltaTime;

        // Check if max is reached or exceeded
        if (fireTimer >= FIRE_TIME)
        {
            // Reset timer
            fireTimer = 0;

            // Turn off laser
            objectsHit = 0; // Reset hits
            lineRenderer.enabled = false; // Disable line renderer

            // Switch to COOLDOWN state
            currentState = States.COOLDOWN;
        }
        // If max not reached or exceeded
        else
        {
            // Fire Laser
            Fire();

        }
    }

    /* CooldownTimer
     * 
     * Updates the cooldown timer and handles timeouts
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void CooldownTimer()
    {
        // Update timer
        cooldownTimer += Time.deltaTime;

        // Check if max is reached or exceeded
        if (cooldownTimer >= COOLDOWN_TIME)
        {
            // Reset timer
            cooldownTimer = 0;

            // Switch to READY state
            currentState = States.READY;
        }
    }

    /* SetUpLineRenderer
     * 
     * Sets up line renderer by setting various settings to desired values
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void SetUpLineRenderer()
    {
        // Set width of line
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // Disable line to not show
        lineRenderer.enabled = false;

        // Set first position of line when drawn
        lineRenderer.SetPosition(0, startPosition);
    }

    /* Fire
     * 
     * Fires the raycast / laser and handles hits
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Fire()
    {
        // Create raycast
        hit = Physics2D.Raycast(transform.position, transform.up, distanceToFire, layerMask);

        // If hit is true
        if (hit)
        {
            // Increase the number of hits
            objectsHit++;
            if (DEBUG_MODE)
            {
                Debug.Log("Hit Count: " + objectsHit);
            }

            // Show line
            lineRenderer.SetPosition(1, new Vector3(0, hit.distance, -1)); // Set end position of line to where the ray hit
            lineRenderer.enabled = true; // Enable line renderer

            // Get game object of hit 
            GameObject hitObject = hit.collider.gameObject;
            
            // Check that hit object has a health component
            if (hitObject.GetComponent<Health>() != null)
            {
                // Get damage to deal
                float applyDamage = 1;

                // Check if we have hit at least one object already
                if (objectsHit > 1)
                {
                    // Decrease damage based on number of objects hit. 2 Hits -> 0.75, 3 Hits -> 0.5 ... until 0 damage dealt
                    applyDamage = 1 - (objectsHit - 1) * 0.25f;
                    if (applyDamage < 0)
                    {
                        applyDamage = 0;
                    }
                }
                // Check that explosion pre fab is present
                if (explosion != null)
                {

                    // Create instance of explosion pre fab
                    GameObject explode = Instantiate(explosion);

                    // Change position of explosion prefab to point of contact
                    explode.transform.position = hit.point;

                    // Change scale of explosion prefab to a more reasonable size
                    explode.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                }
                // Get health component and deal damage
                hitObject.GetComponent<Health>().TakeDamage(applyDamage);
            }
            else
            {
                Debug.LogWarning("No health component on hit object");
            }

        }
        // Nothing was hit
        else
        {
            // Draw and enable line to full extent of laser
            lineRenderer.SetPosition(1, new Vector3(0, distanceToFire, -1));
            lineRenderer.enabled = true;
        }
    }

    /* PlaySound
     * 
     * Plays the sound connected to the audio source
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void PlaySound()
    {
        // Check that audio source is not null
        if (audioSource != null)
        {
            // Play audio
            audioSource.Play();
        }
    }
}

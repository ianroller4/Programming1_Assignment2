using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player
 * 
 * Player controller
 * 
 */
public class Player : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;
    private AudioSource audioSource;

    // --- Input ---
    private Vector2 playerInput;

    // --- Force Variables ---
    public float linearForce = 1f;
    public float rotationForce = 0.1f;
    private float baseBrakeDrag; // Linear drag when not breaking
    private float baseRadialBrakeDrag; // Radial drag when not breaking
    private float activeBrakeDrag = 5f; // Linear drag when breaking
    private float activeRadialBrakeDrag = 5f; // Radial drag when breaking

    // --- Bools ---
    private bool reversing = false; // Am I reversing or not

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
        // Get components
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // Get base drag values
        baseBrakeDrag = rb.drag;
        baseRadialBrakeDrag = rb.angularDrag;
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
        // Listen for input
        ListenForInput();
    }

    /* FixedUpdate
     * 
     * Called once at fixed intervals
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void FixedUpdate()
    {
        // Move the player
        Move();
    }

    /* ListenForInput
     * 
     * Updates playerInput by listening to input
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ListenForInput()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    /* Move
     * 
     * Move the player using physics based on player input
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void Move()
    {
        // If no player input stop brake sound
        if (playerInput.x == 0 && playerInput.y == 0)
        {
            //Stop reverse sound
            audioSource.Stop();
        }

        // If player moving forward
        if (playerInput.y > 0)
        {
            // Stop reverse sound
            audioSource.Stop();

            // Create vector with just y input
            Vector2 yVector = new Vector2(0, playerInput.y);

            // Add force in direction of of y vector relative to player
            rb.AddRelativeForce(linearForce * yVector);

            // Set drag values to base values
            rb.angularDrag = baseRadialBrakeDrag;
            rb.drag = baseBrakeDrag;

            // Not reversing
            reversing = false;
        }
        else
        {
            // Player y is negative check if moving slow enough to transition to reversing
            if (rb.velocity.magnitude < 0.001 && playerInput.y < 0 && reversing == false)
            {
                // Create vector with just y input
                Vector2 yVector = new Vector2(0, playerInput.y);

                // Add force in direction of of y vector relative to player
                rb.AddRelativeForce(linearForce * yVector);

                // If audio source is not playing play the reverse sound
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                // Player is reversing
                reversing = true;
            } 
            // Player moving slow enough so player is reversing
            else if (reversing == true)
            {
                // Create vector with just y input
                Vector2 yVector = new Vector2(0, playerInput.y);

                // Add force in direction of of y vector relative to player
                rb.AddRelativeForce(linearForce * yVector);

                // If audio source is not playing play the reverse sound
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            // Player y is negative but still moving fast enough so apply breaks
            else
            {
                // Linear interpolate between base break values and active break values
                rb.angularDrag = Mathf.Lerp(baseRadialBrakeDrag, activeRadialBrakeDrag, Mathf.Abs(playerInput.y));
                rb.drag = Mathf.Lerp(baseBrakeDrag, activeBrakeDrag, Mathf.Abs(playerInput.y));
            }
            
        }

        // Add torque based on x input, -1 is so direction is more intuitive
        rb.AddTorque(rotationForce * playerInput.x * -1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Explosion
 * 
 * Simulates an explosion effect 
 * 
 */
public class Explosion : MonoBehaviour
{
    // --- Component References ---
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // --- Sprite Timer ---
    private float spriteTimer = 0f;
    private float MAX_SPRITE_TIME = 0.1f;

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
        // Get audio source component
        audioSource = GetComponent<AudioSource>();

        // Get sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Randomly rotate the sprite to look different
        float rotation = Random.Range(0, 90); // Get float between 0 and 90 degrees
        transform.Rotate(0, 0, rotation); // Rotate on z-axis
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
        // Update timer
        SpriteTimer();

        // Check if audio is still playing before destroying
        if (!audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    /* Sprite Timer
     * 
     * Timer for disabling sprite renderer so that explosion sprite does not stay on too long
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void SpriteTimer()
    {
        // Update timer
        spriteTimer += Time.deltaTime;

        // Check if max is reached or exceeded
        if (spriteTimer >= MAX_SPRITE_TIME)
        {
            // Disable sprite renderer
            spriteRenderer.enabled = false;
        }
    }
}

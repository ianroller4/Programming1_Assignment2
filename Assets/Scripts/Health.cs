using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

/* Health
 * 
 * Handles health and dealing damage to objects with health
 * 
 */
public class Health : MonoBehaviour
{
    // --- Health Variables ---
    private float currentHP;
    public float MAX_HP = 3f;

    // --- Damage Sprite ---
    public Sprite damaged;
    private Sprite original;
    private SpriteRenderer spriteRenderer;

    // --- Switches ---
    public bool DEBUG_MODE = false; // Used to enable or disable debug options

    // --- Explosion Pre Fab ---
    public GameObject explosionPrefab;

    // --- Wreckage Pre Fab ---
    public GameObject wreckagePrefab;

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
        // Get sprite renderer component from child called Art
        spriteRenderer = transform.Find("Art").gameObject.GetComponent<SpriteRenderer>();

        // Get the original sprite 
        original = spriteRenderer.sprite;

        // Set currentHP to MAX_HP
        currentHP = MAX_HP;
    }

    /* DamageFlicker
     * 
     * Coroutine for flickering sprite when damage taken
     * 
     * Parameters: None
     * 
     * Return: IEnumerator, allows function to be iterated over time
     * 
     */
    private IEnumerator DamageFlicker()
    {
        // If the damaged sprite is not null, so only the player has this
        if (damaged != null)
        {
            // Change sprite to damaged sprite
            spriteRenderer.sprite = damaged;

            // Wait for 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            // Change sprite back to original sprite
            spriteRenderer.sprite = original;
        }
    }

    /* TakeDamage
     * 
     * Deals damage to the health of the game object, and handles deaths
     * 
     * Parameters: float damage, the damage to deal
     * 
     * Return: bool result, whether or not health reached zero
     * 
     */
    public bool TakeDamage(float damage)
    {
        bool result = false;
        
        // Apply damage to HP
        currentHP -= damage;

        // If health is greater than 0 and damaged sprite is present
        if (currentHP > 0 && damaged != null)
        {
            // Start DamageFlicker coroutine
            StartCoroutine(DamageFlicker());
        }
        if (DEBUG_MODE)
        {
            Debug.Log("Hit!");
            Debug.Log("Current HP: " + currentHP);
        }

        // If HP is zero or less
        if (currentHP <= 0)
        {
            result = true;

            // Death
            DeathFromDamage();
        }

        return result;
    }

    /* Heal
     * 
     * Heals a health component of a game object by the specified amount up to max hp
     * 
     * Parameters: float heal, the amount to heal
     * 
     * 
     * Return: None
     * 
     */
    public void Heal(float heal)
    {
        // Heal
        currentHP += heal;

        // Check if greater than max
        if (currentHP > MAX_HP)
        {
            // Set to max
            currentHP = MAX_HP;
        }
        if (DEBUG_MODE)
        {
            Debug.Log("Healed! Current HP now at: " + currentHP);
        }
    }

    /* DeathFromDamage
     * 
     * Destroys gameobject and creates effects
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void DeathFromDamage()
    {
        // If explosion pre fab is present
        if (explosionPrefab != null)
        {
            // Create instance of explosion
            GameObject explode = Instantiate(explosionPrefab);

            // Set position of explosion to object position
            explode.transform.position = transform.position;
        }
        // if wreckage pre fab is present
        if (wreckagePrefab != null)
        {
            Wreckage();
        }
        // Destroy the game object
        Destroy(gameObject);
    }

    /* Wreckage
     * 
     * Creates the wreckage game object and starts its drifting
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Wreckage()
    {
        // Create instance of wreckage pre fab
        GameObject wreckage = Instantiate(wreckagePrefab);

        // Get rigidbody
        Rigidbody2D rb = wreckage.GetComponent<Rigidbody2D>();

        // Set velocity to the same as player before death
        rb.velocity = gameObject.GetComponent<Rigidbody2D>().velocity;

        // Add torque
        rb.AddTorque(20);

        // Add impulse force in random direction
        rb.AddForce((new Vector2(Random.Range(-1, 1), Random.Range(-1, 1))) * 20, ForceMode2D.Impulse);
    }
}

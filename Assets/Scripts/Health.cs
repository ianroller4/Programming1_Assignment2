using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.Find("Art").gameObject.GetComponent<SpriteRenderer>();
        original = spriteRenderer.sprite;
        currentHP = MAX_HP;
    }

    private IEnumerator DamageFlicker()
    {
        if (damaged != null)
        {
            spriteRenderer.sprite = damaged;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = original;
        }
    }

    public bool TakeDamage(float damage)
    {
        bool result = false;
        currentHP -= damage;
        if (currentHP > 0 && damaged != null)
        {
            StartCoroutine(DamageFlicker());
        }
        if (DEBUG_MODE)
        {
            Debug.Log("Hit!");
            Debug.Log("Current HP: " + currentHP);
        }

        if (currentHP <= 0)
        {
            result = true;
            DeathFromDamage();
        }

        return result;
    }

    public void DeathFromDamage()
    {
        Destroy(gameObject);
    }
}

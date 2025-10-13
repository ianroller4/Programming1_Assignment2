using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // --- Health Variables ---
    private float currentHP;
    public float MAX_HP = 3f;

    // --- Switches ---
    public bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = MAX_HP;
    }

    public bool TakeDamage(float damage)
    {
        bool result = false;
        currentHP -= damage;
        if (DEBUG_MODE)
        {
            Debug.Log("Hit!");
            Debug.Log("Current HP: " + currentHP);
        }

        if (currentHP < 0)
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

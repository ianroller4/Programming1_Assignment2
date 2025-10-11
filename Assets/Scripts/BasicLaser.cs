using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicLaser : MonoBehaviour
{
    // --- Timer Variables ---
    public float FIRE_TIME = 1f;
    private float fireTimer = 0;
    public float COOLDOWN_TIME = 1f;
    private float cooldownTimer = 0;

    // --- Raycast Variables ---
    public float distanceToFire = 4f;
    public LayerMask layerMask;
    RaycastHit2D laser;

    // --- State Enum ---
    private enum States {
        READY,   // Will fire when button pressed 
        ACTIVE,  // Is firing, cannot fire again
        COOLDOWN // Is not firing, cannot fire yet
    };

    States currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = States.READY;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.READY)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Fire Laser
                Fire();
                currentState = States.ACTIVE;
            }
        }
        else if (currentState == States.ACTIVE)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= FIRE_TIME)
            {
                // Stop Laser
                // Stop art
                fireTimer = 0;
                currentState = States.COOLDOWN;
            }
            else
            {
                // Fire Laser
                Fire();
            }
        }
        else if (currentState == States.COOLDOWN)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= COOLDOWN_TIME)
            {
                cooldownTimer = 0;
                currentState = States.READY;
            }
        }

    }

    private void Fire()
    {
        laser = Physics2D.Raycast(transform.position, transform.up, distanceToFire, layerMask);
        Debug.DrawRay(transform.position, transform.up, Color.red);
    }
}

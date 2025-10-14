using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicLaser : MonoBehaviour
{
    // --- Timer Variables ---
    public float FIRE_TIME = 0.25f;
    private float fireTimer = 0;
    public float COOLDOWN_TIME = 1f;
    private float cooldownTimer = 0;

    // --- Raycast Variables ---
    public float distanceToFire = 4f;
    public LayerMask layerMask;
    private RaycastHit2D hit;

    private int objectsHit = 0;

    // --- State Enum ---
    private enum States {
        READY,   // Will fire when button pressed 
        ACTIVE,  // Is firing, cannot fire again
        COOLDOWN // Is not firing, cannot fire yet
    };
    States currentState;

    // --- Line Renderer ---
    private LineRenderer lineRenderer;
    private Vector3 startPosition = new Vector3(0, 0.5f, -1);

    // --- Switches ---
    public bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = States.READY;
        lineRenderer = GetComponent<LineRenderer>();
        SetUpLineRenderer();
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
                fireTimer = 0;
                objectsHit = 0;
                currentState = States.COOLDOWN;
                lineRenderer.enabled = false;
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

    private void SetUpLineRenderer()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, startPosition);
    }

    private void Fire()
    {
        hit = Physics2D.Raycast(transform.position, transform.up, distanceToFire, layerMask);

        if (hit)
        {
            objectsHit++;
            if (DEBUG_MODE)
            {
                Debug.Log("Hit Count: " + objectsHit);
            }
            // Show line
            lineRenderer.SetPosition(1, new Vector3(0, hit.distance, -1));
            lineRenderer.enabled = true;

            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.GetComponent<Health>() != null)
            {
                float applyDamage = 1;
                if (objectsHit > 1)
                {
                    applyDamage = 1 - (objectsHit - 1) * 0.25f;
                    if (applyDamage < 0)
                    {
                        applyDamage = 0;
                    }
                }
                hitObject.GetComponent<Health>().TakeDamage(applyDamage);
            }

        }
        else
        {
            lineRenderer.SetPosition(1, new Vector3(0, distanceToFire, -1));
            lineRenderer.enabled = true;
        }
       
    }
}

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
    public bool DEBUG_MODE = false;

    // Start is called before the first frame update
    void Start()
    {
        dangers = new List<GameObject>();
        redRing = GetComponent<SpriteRenderer>();
        redRing.enabled = false;
    }

    private void Update()
    {
        if (alertTimerStarted)
        {
            alertTimer += Time.deltaTime;
            if (alertTimer >= ALERT_TIME_MAX)
            {
                redRing.enabled = false;
                alertTimer = 0;
            }
        }

        if (DEBUG_MODE)
        {
            DrawDangerLines();
        }
    }

    private void DrawDangerLines()
    {
        Vector3 position = transform.position;
        for (int i = 0; i < dangers.Count; i++)
        {
            Debug.DrawLine(position, dangers[i].transform.position, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided != null)
        {
            if (collided.layer == 10)
            {
                dangers.Add(collided);
                alertTimerStarted = true;
                redRing.enabled = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided != null)
        {
            if (collided.layer == 10)
            {
                if (dangers.Contains(collided))
                {
                    dangers.Remove(collided);
                }
            }
        }
    }
}

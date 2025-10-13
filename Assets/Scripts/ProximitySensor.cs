using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    // --- Proximity Alert Variables ---
    private SpriteRenderer redRing;
    private float alertTimer = 0;
    private bool alertTimerStarted = false;
    public float ALERT_TIME_MAX = 1f;

    // --- List of Dangers ---
    List<GameObject> dangers;

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

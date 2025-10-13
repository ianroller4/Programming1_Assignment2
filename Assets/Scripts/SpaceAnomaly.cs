using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceAnomaly : MonoBehaviour
{
    Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(Random.Range(50, 100));
        Vector2 forceDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        rb.AddForce(forceDirection * Random.Range(20, 40), ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedWith = collision.gameObject;

        if (collidedWith != null)
        {
            if (collidedWith.layer == 6)
            {
                if (collidedWith.GetComponent<Rigidbody2D>() != null)
                {
                    Rigidbody2D rbCollided = collidedWith.GetComponent<Rigidbody2D>();
                    rbCollided.AddTorque(Random.Range(50, 100));
                    Vector2 forceDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
                    rbCollided.AddForce(forceDirection * Random.Range(50, 100));
                }
            }
        }
    }
}

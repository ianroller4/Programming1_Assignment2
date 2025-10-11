using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DamageOnCollision : MonoBehaviour
{
    private float damageToDeal = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedWith = collision.gameObject;

        if (collidedWith != null)
        {
            if (collidedWith.layer == 6)
            {
                if (collidedWith.GetComponent<Health>() != null)
                {
                    collidedWith.GetComponent<Health>().TakeDamage(damageToDeal);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;

    // --- Input ---
    private Vector2 playerInput;

    // --- Force Variables ---
    public float linearForce = 0.5f;
    public float rotationForce = 0.1f;
    private float baseBrakeDrag;
    private float baseRadialBrakeDrag;
    private float activeBrakeDrag = 5f;
    private float activeRadialBrakeDrag = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseBrakeDrag = rb.drag;
        baseRadialBrakeDrag = rb.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        ListenForInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void ListenForInput()
    {
        playerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void Move()
    {
        if (playerInput.y > 0)
        {
            Vector2 yVector = new Vector2(0, playerInput.y);
            rb.AddRelativeForce(linearForce * yVector);

            rb.angularDrag = baseRadialBrakeDrag;
            rb.drag = baseBrakeDrag;
        }
        else
        {
            rb.angularDrag = Mathf.Lerp(baseRadialBrakeDrag, activeRadialBrakeDrag, Mathf.Abs(playerInput.y));
            rb.drag = Mathf.Lerp(baseBrakeDrag, activeBrakeDrag, Mathf.Abs(playerInput.y));
        }

        rb.AddTorque(rotationForce * playerInput.x * -1f);
    }
}

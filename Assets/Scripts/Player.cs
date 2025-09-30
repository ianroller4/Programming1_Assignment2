using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;

    // --- Input Variables ---
    private float xInput;
    private float yInput;

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
        ProcessInput();
    }

    public void ListenForInput()
    {
        yInput = Input.GetAxis("Vertical");
        xInput = Input.GetAxis("Horizontal");
    }

    public void ProcessInput()
    {
        Vector2 yVector = new Vector2(0, yInput);

        rb.AddRelativeForce(linearForce * yVector);

        if (yInput >= 0)
        {
            rb.angularDrag = baseRadialBrakeDrag;
            rb.drag = baseBrakeDrag;
        }
        else
        {
            rb.angularDrag = activeRadialBrakeDrag;
            rb.drag = activeBrakeDrag;
        }

            rb.AddTorque(rotationForce * xInput * -1f);
    }
}

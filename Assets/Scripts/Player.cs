using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // --- Component References ---
    private Rigidbody2D rb;
    private AudioSource audioSource;

    // --- Input ---
    private Vector2 playerInput;

    // --- Force Variables ---
    public float linearForce = 1f;
    public float rotationForce = 0.1f;
    private float baseBrakeDrag;
    private float baseRadialBrakeDrag;
    private float activeBrakeDrag = 5f;
    private float activeRadialBrakeDrag = 5f;

    // --- Bools ---
    private bool reversing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        if (playerInput.x == 0 && playerInput.y == 0)
        {
            //Stop Sound
            audioSource.Stop();
        }
        if (playerInput.y > 0)
        {
            // Stop Sound
            audioSource.Stop();
            Vector2 yVector = new Vector2(0, playerInput.y);
            rb.AddRelativeForce(linearForce * yVector);

            rb.angularDrag = baseRadialBrakeDrag;
            rb.drag = baseBrakeDrag;
            reversing = false;
        }
        else
        {
            if (rb.velocity.magnitude < 0.1 && playerInput.y < 0 && reversing == false)
            {
                rb.angularDrag = baseRadialBrakeDrag;
                rb.drag = baseBrakeDrag;
                Vector2 yVector = new Vector2(0, playerInput.y);
                rb.AddRelativeForce(linearForce * yVector);
                reversing = true;
                // Play Sound if sound not playing
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            } 
            else if (reversing == true)
            {
                Vector2 yVector = new Vector2(0, playerInput.y);
                rb.AddRelativeForce(linearForce * yVector);
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                rb.angularDrag = Mathf.Lerp(baseRadialBrakeDrag, activeRadialBrakeDrag, Mathf.Abs(playerInput.y));
                rb.drag = Mathf.Lerp(baseBrakeDrag, activeBrakeDrag, Mathf.Abs(playerInput.y));
            }
            
        }

        rb.AddTorque(rotationForce * playerInput.x * -1f);
    }
}

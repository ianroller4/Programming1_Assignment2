using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoundaryWarp : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 topRightScreen;
    private Vector2 bottomLeftScreen;

    private Vector3 positionOnScreen;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        topRightScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        bottomLeftScreen = Camera.main.ScreenToWorldPoint(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        updatePosition();

        boundaryWrap();  
    }

    private void updatePosition()
    {
        // Get position of object in the screen in pixels
        positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void boundaryWrap()
    {
        // If passing left side of screen and moving left
        if (positionOnScreen.x <= 0 && rb.velocity.x < 0)
        {
            transform.position = new Vector2(topRightScreen.x, transform.position.y);
        }
        else if (positionOnScreen.x >= Screen.width && rb.velocity.x > 0)
        {
            transform.position = new Vector2(bottomLeftScreen.x, transform.position.y);
        }
        else if (positionOnScreen.y <= 0 && rb.velocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, topRightScreen.y);
        }
        else if (positionOnScreen.y >= Screen.height && rb.velocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, bottomLeftScreen.y);
        }
    }
}

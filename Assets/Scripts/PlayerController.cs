using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    [HideInInspector] public bool isGoingLeft;
    private Vector2 goingLeft, goingRight, isUp, isDown, currentPosition;
    private bool grounded;

    private void Awake() // grab references
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        goingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        goingRight = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // direction going
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // direction facing
        if (horizontalInput > 0.01f)
        {
            transform.localScale = goingRight;
        }
        else if (horizontalInput < -0.0f)
        {
            transform.localScale = goingLeft;
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
            grounded = true;
    }

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

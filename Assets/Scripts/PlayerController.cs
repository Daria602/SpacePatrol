using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerNumber
{
    PlayerOne = 0,
    PlayerTwo = 1,
}
public class PlayerController : MonoBehaviour
{
    public PlayerNumber PlayerNr;
    private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float groundDistance;

    //The layers that are considered to be ground
    [SerializeField] private LayerMask groudMask;
    private SpriteRenderer _spriteRenderer;

    //[HideInInspector] public bool isGoingLeft;
    // private Vector2 goingLeft, goingRight, isUp, isDown, currentPosition;


    //function for changing the gravity
    public bool IsUpsideDown
    {
        get => _isUpsideDown;
        set
        {
            if (value == _isUpsideDown) return;
            _isUpsideDown = value;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y*-1, transform.localScale.z);
            //Change the gravity scaling 
            _rigidbody.gravityScale *= -1;
        }
    }

    private bool _isUpsideDown = false;
    //function for changing the x axis orientation
    public bool FacingRight
    {
        get => _facingRight;
        set
        {
            if (value == _facingRight) return;
            _facingRight = value;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private bool _facingRight;
    private bool _isGrounded;
    
    RaycastHit2D hit;

    private void Awake() // grab references
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _facingRight = Mathf.Sign(transform.localScale.x) > 0 ? true : false;
        _isUpsideDown= Mathf.Sign(transform.localScale.y) < 0 ? true : false;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //goingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        //goingRight = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = 0f;
        bool shouldJump = false;
        switch (PlayerNr)
        {
            case PlayerNumber.PlayerOne:
                horizontalInput = Input.GetAxis("Horizontal_One");
                shouldJump = Input.GetButtonDown("Jump_One");
                break;
            case PlayerNumber.PlayerTwo:
                horizontalInput = Input.GetAxis("Horizontal_Two");
                shouldJump = Input.GetButtonDown("Jump_Two");
                break;
        }

        // direction going
        _rigidbody.velocity = new Vector2(horizontalInput * speed, _rigidbody.velocity.y);

        FacingRight = horizontalInput >= 0 ? true : false;

        hit = Physics2D.Raycast(transform.position, Vector2.down*(IsUpsideDown ? -1 : 1), groundDistance, groudMask);
        _isGrounded = hit.collider != null;

       if (shouldJump && _isGrounded)
        Jump();

       //For debug 
       ///if u press tab it will change the gravity
       if(Input.GetKeyDown(KeyCode.Tab))
        {
            IsUpsideDown = !IsUpsideDown;
        }
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, speed * (IsUpsideDown ? -1 : 1));
    }

    //This type collision detection is bad because if we hit our head with a platform we will be grounded
    //Use raycast instead
    /*

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform")
            _grounded = true;
    }
    */

    private void OnMouseDown()
    {
        if(_spriteRenderer)
        _spriteRenderer.color = Color.red;
    }

    private void OnMouseUp()
    {
        if (_spriteRenderer)
            _spriteRenderer.color = Color.white;
    }
    private void OnDrawGizmosSelected()
    {
        //for debug
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));
    }
}

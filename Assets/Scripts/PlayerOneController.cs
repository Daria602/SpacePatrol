using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerOneController : MonoBehaviour
{
    public Animator animator;

    private Rigidbody2D _rigidbody;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private float groundDistance;

    //The layers that are considered to be ground
    [SerializeField] private LayerMask groudMask;

    private bool _facingRight;
    private bool _isGrounded;
    private bool _isUpsideDown = false;
    RaycastHit2D hit;

    private void Awake() 
    {
        // grab references
        _rigidbody = GetComponent<Rigidbody2D>();
        _facingRight = Mathf.Sign(transform.localScale.x) > 0 ? true : false;
        _isUpsideDown= Mathf.Sign(transform.localScale.y) < 0 ? true : false;
    }

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal_One");
        bool shouldJump = Input.GetButtonDown("Jump_One");

        // direction going
        _rigidbody.velocity = new Vector2(horizontalInput * runningSpeed, _rigidbody.velocity.y);
        animator.SetBool("isRunning", (_rigidbody.velocity.x == 0) ? false : true);


        FacingRight = horizontalInput >= 0 ? true : false;

        hit = Physics2D.Raycast(transform.position, Vector2.down*(IsUpsideDown ? -1 : 1), groundDistance, groudMask);
        _isGrounded = hit.collider != null;
        Debug.Log(hit.collider);

       if (shouldJump && _isGrounded)
        Jump();

        if (_isGrounded)
            animator.SetBool("isJumping", false);
        else
            animator.SetBool("isJumping", true);

        //For debug 
        ///if u press tab it will change the gravity
        if (Input.GetKeyDown(KeyCode.Tab))
       {
            IsUpsideDown = !IsUpsideDown;
       }
    }

    //function for changing the gravity
    public bool IsUpsideDown
    {
        get => _isUpsideDown;
        set
        {
            if (value == _isUpsideDown) return;
            _isUpsideDown = value;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            //Change the gravity scaling 
            _rigidbody.gravityScale *= -1;
        }
    }

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

    private void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingSpeed * (IsUpsideDown ? -1 : 1));
    }

    /*
    private void OnDrawGizmosSelected()
    {
        //for debug
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));
    }
    */
}

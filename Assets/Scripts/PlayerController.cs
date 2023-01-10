using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    public Animator animator;

    private Rigidbody2D _rigidbody;
    private DamageReceiver _damageReceiver;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private float groundDistance;

    //The layers that are considered to be ground
    [SerializeField] private LayerMask groundMask;

    private bool _facingRight;
    private bool _isGrounded;
    private bool _isUpsideDown = false;
    RaycastHit2D hit;

    public Vector3 lastCheckpoint;
    public bool shouldTP;

    private bool doubleJump;

    bool isTouchingFront;
    public Transform frontCheck;
    public Transform groundCheck;
    private bool wallSliding;
    public float checkRadius;
    public float wallSlidingSpeed;



    private void Awake() 
    {
        // grab references
        _rigidbody = GetComponent<Rigidbody2D>();
        _facingRight = Mathf.Sign(transform.localScale.x) > 0 ? true : false;
        _isUpsideDown= Mathf.Sign(transform.localScale.y) < 0 ? true : false;
        _damageReceiver = GetComponent<DamageReceiver>();

        if(_damageReceiver!=null)
        {
            _damageReceiver.OnTakeDamage += OnDamageTaken;
            _damageReceiver.OnDead += OnDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

        

        float horizontalInput = Input.GetAxisRaw("Horizontal_Two");
        bool shouldJump = Input.GetButtonDown("Jump");

        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius*5, groundMask);
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);
        bool shouldWallSlide = isTouchingFront && !_isGrounded && horizontalInput != 0;

        if (horizontalInput > 0)
        {
            FacingRight = true;
        } 
        else if (horizontalInput < 0)
        {
            FacingRight = false;
        }



        if (shouldWallSlide)
        {
            wallSliding = true;
        } 
        else
        {
            wallSliding = false;
        }

        _rigidbody.velocity = new Vector2(horizontalInput * runningSpeed, wallSliding ? Mathf.Clamp(_rigidbody.velocity.y, -wallSlidingSpeed, float.MaxValue) : _rigidbody.velocity.y);


        bool isRunning = horizontalInput != 0 && _isGrounded && !wallSliding;
        animator.SetBool("isRunning", isRunning);

        

        

        // reset double jump
       if(_isGrounded && !Input.GetButton("Jump"))
       {
            doubleJump = false;
       }

       if (shouldJump)
       {
            if (_isGrounded || doubleJump)
            {
                Jump();
                doubleJump = !doubleJump;
            }
       }


       //this is bad because you can be in air but not jumping. use a trigger on jump instead.
        if (_isGrounded)
            animator.SetBool("isJumping", false);
        else
            animator.SetBool("isJumping", true);

        
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
        animator.SetBool("isJumping", true);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingSpeed);
    }

    private void OnDamageTaken(int obj)
    {
        
        // make animation of taking damage
        animator.SetBool("isHurt", true);

        // teleport to the last checkpoint
        if (shouldTP)
            transform.position = lastCheckpoint;
    }
    private void OnDead()
    {
        //this is called when the hp reaches 0
        animator.SetBool("isDead", true);

    }
    
    private void OnDrawGizmosSelected()
    {
        //for debug
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));
    }

    public void hurtAnimationPassed()
    {
        animator.SetBool("isHurt", false);
    }

    public void DeathAnimationPassed()
    {
        FindObjectOfType<GameManager>().PlayerIsDead = true;
        Destroy(gameObject);
        
    }
    
}

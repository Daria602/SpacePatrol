using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    public Animator animator;

    private Rigidbody2D rigidBody;
    private DamageReceiver damageReceiver;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private float groundDistance;

    //The layers that are considered to be ground
    [SerializeField] private LayerMask groundMask;

    private bool facingRight;
    private bool isGrounded;

    public Vector3 lastCheckpoint;
    public bool shouldTP;

    private bool doubleJump;

    bool isTouchingFront;
    public Transform frontCheck;
    public Transform groundCheck;
    private bool wallSliding;
    public float checkRadius;
    public float wallSlidingSpeed;

    private float horizontalInput;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer trailRenderer;



    private void Awake() 
    {
        // grab references
        rigidBody = GetComponent<Rigidbody2D>();
        facingRight = Mathf.Sign(transform.localScale.x) > 0 ? true : false;
        damageReceiver = GetComponent<DamageReceiver>();

        if(damageReceiver!=null)
        {
            damageReceiver.OnTakeDamage += OnDamageTaken;
            damageReceiver.OnDead += OnDead;
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        
        // Get the horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal_Two");

        // Check if the character is touching the wall and if they are grounded
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundMask);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);

        // Depending on horizontal input, turn the character left or right
        Flip();

        bool shouldJump = Input.GetButtonDown("Jump");
        bool isAllowedToJump = false;
        if (shouldJump)
        {
            isAllowedToJump = CheckIfAllowedToJump();
        }

        if (isAllowedToJump)
        {
            animator.SetBool("isJumping", true);
            Jump();
        }



        if (ShouldWallSlide())
        {
            wallSliding = true;
            animator.SetBool("isWallsliding", true);
            
        } 
        else
        {
            wallSliding = false;
            animator.SetBool("isWallsliding", false);
        }

        rigidBody.velocity = new Vector2(horizontalInput * runningSpeed, wallSliding ? Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue) : rigidBody.velocity.y);

        animator.SetBool("isRunning", ShouldBeRunning());

        // this is bad because you can be in air but not jumping. use a trigger on jump instead.
        //if (isGrounded)
        //    animator.SetBool("isJumping", false);
        //else
        //    animator.SetBool("isJumping", true);

        if ((rigidBody.velocity.y > -0.1 && rigidBody.velocity.y < 0.1) || wallSliding)
        {
            animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            //Debug.Log("Got here");
            StartCoroutine(Dash());
        }

    }

    private IEnumerator Dash()
    {
        if (Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundMask))
        {
            Debug.Log("cONDITION WAS MET");
            yield break;
        }
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        Debug.Log(rigidBody.velocity);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        rigidBody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void Flip()
    {
        if (horizontalInput > 0)
        {
            FacingRight = true;
        }
        else if (horizontalInput < 0)
        {
            FacingRight = false;
        }
    }

    private bool CheckIfAllowedToJump()
    {
        if (isGrounded)
        {
            doubleJump = true;
            return true;
        }
        if (isTouchingFront)
        {
            doubleJump = true;
            return true;
        }
        // check if allowed double jump while not touching the front and while not grounded
        if (doubleJump)
        {
            doubleJump = false;
            return true;
        }
        return false;
    }

    private bool ShouldWallSlide()
    {
        return isTouchingFront && !isGrounded && horizontalInput != 0;
    }

    private bool ShouldBeRunning()
    {
        return horizontalInput != 0 && isGrounded && !wallSliding;
    }


    //function for changing the x axis orientation
    public bool FacingRight
    {
        get => facingRight;
        set
        {
            if (value == facingRight) return;
            facingRight = value;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Jump()
    {
        animator.SetBool("isJumping", true);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
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

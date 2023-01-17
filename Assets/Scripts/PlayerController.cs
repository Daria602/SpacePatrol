using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{

    private Animator animator;
    public Transform groundCheck;


    private Rigidbody2D rigidBody;
    private DamageReceiver damageReceiver;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private float groundDistance;

    //The layers that are considered to be ground
    [SerializeField] private LayerMask groundMask;

    private bool facingRight;

    public Vector3 lastCheckpoint;
    public bool shouldTP;

    

    
    
    public float checkRadius;
    public float wallCheckRadius = 0.5f;


    private float horizontalInput;


    // Dashing mechanics
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;


    private bool isWallSliding = false;
    public float wallSlidingSpeed;
    public Transform frontCheck;


    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 6f);

    private bool doubleJump;

    //enum PlayerState
    //{
    //    Idle,
    //    Running,
    //    Jumping,
    //    Falling,
    //    WallSliding
    //}

    //private PlayerState currentState = PlayerState.Idle;


    private void Awake() 
    {
        // grab references
        rigidBody = GetComponent<Rigidbody2D>();
        facingRight = Mathf.Sign(transform.localScale.x) > 0 ? true : false;
        damageReceiver = GetComponent<DamageReceiver>();
        animator = GetComponentInChildren<Animator>();

        if(damageReceiver!=null)
        {
            damageReceiver.OnTakeDamage += OnDamageTaken;
            damageReceiver.OnDead += OnDead;
        }
    }

    private bool isJumping = false;

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        
        horizontalInput = Input.GetAxisRaw("Horizontal_Two");

        if ((IsGrounded() || IsWalled()) && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                isJumping = true;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
                doubleJump = !doubleJump;
            }
            
        }
        if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0f)
        {
            
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        }

        animator.SetFloat("yVelocity", rigidBody.velocity.y);

        

        //if (IsGrounded())
        //{
        //    animator.SetBool("isJumping", false);
        //}




        WallSlide();
        WallJump();
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (!isWallJumping)
        {
            // Depending on horizontal input, turn the character left or right
            Flip();
        }

        /*  ANIMATIONS  */
        if (IsGrounded() && horizontalInput != 0)
        {
            setAnimation("isRunning");
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        if (!IsGrounded() && isJumping)
        {
            setAnimation("isJumping");
        }
        else
        {
            animator.SetBool("isJumping", false);
        }

        if (isWallSliding)
        {
            setAnimation("isWallsliding");
        } else
        {
            animator.SetBool("isWallsliding", false);
        }




    }



    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            Debug.Log("Wall Jumpin");
            isWallJumping = true;
            rigidBody.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                FacingRight = !FacingRight;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void setAnimation(string animationBool)
    {
        string[] animationBools = { "isJumping", "isRunning" };
        for (int i = 0; i < animationBools.Length; i++)
        {
            animator.SetBool(animationBools[i], false);
        }
        animator.SetBool(animationBool, true);
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Run()
    {
        if (IsGrounded() && horizontalInput != 0f)
        {
            rigidBody.velocity = new Vector2(horizontalInput * runningSpeed, rigidBody.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(frontCheck.position, 0.2f, groundMask); 
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontalInput != 0f)
        {
            isWallSliding = true;
            isJumping = false;
            rigidBody.velocity = new Vector2(0f, Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            rigidBody.velocity = new Vector2(horizontalInput * runningSpeed, rigidBody.velocity.y);
            isWallSliding = false;
            if (!IsGrounded())
            {
                isJumping = true;
            }
        }
    }

    private IEnumerator Dash()
    {
        if (Physics2D.OverlapCircle(frontCheck.position, checkRadius, groundMask))
        {
            yield break;
        }
        canDash = false;
        isDashing = true;
        float originalGravity = rigidBody.gravityScale;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
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
        if (IsGrounded() && Input.GetButtonDown("Jump"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingSpeed);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public enum PlayerNumber
    {
        PlayerOne=0,
        PlayerTwo=1,
    }

    public Animator animator;
    public PlayerNumber playerNumber;

    private Rigidbody2D _rigidbody;
    private DamageReceiver _damageReceiver;
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
        float horizontalInput = 0;
        bool shouldJump = false;

        switch (playerNumber)
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
        _rigidbody.velocity = new Vector2(horizontalInput * runningSpeed, _rigidbody.velocity.y);
           
        animator.SetBool("isRunning", horizontalInput!=0);


        FacingRight = horizontalInput >= 0;

        hit = Physics2D.Raycast(transform.position, Vector2.down*(IsUpsideDown ? -1 : 1), groundDistance, groudMask);
        _isGrounded = hit.collider != null;

       if (shouldJump && _isGrounded)
        Jump();

       //this is bad because you can be in air but not jumping. use a trigger on jump instead.
        if (_isGrounded)
            animator.SetBool("isJumping", false);
        else
            animator.SetBool("isJumping", true);

        //For debug 
        ///if u press tab it will change the gravity
        if (playerNumber == PlayerNumber.PlayerOne && Input.GetKeyDown(KeyCode.RightControl))
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
        animator.SetBool("isJumping", true);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingSpeed * (IsUpsideDown ? -1 : 1));
    }

    private void OnDamageTaken(int obj)
    {

    }
    private void OnDead()
    {
        //this is called when the hp reaches 0
    }
    
    private void OnDrawGizmosSelected()
    {
        //for debug
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundDistance, transform.position.z));
    }
    
}

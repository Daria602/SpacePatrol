using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle = 1,
        MoveTowardsPlayer = 2,
        AttacksPlayer = 3
    }
    public bool canFly;
    // Idle speed of the enemy
    [SerializeField] private float speed;

    // When the enemy sees the player, if moves towards it faster
    [SerializeField] private float speedIncrease;

    // Positions to move to in Idle state
    [SerializeField] private Vector3[] positions;

    public Transform player;
    public float playerVisibleRange;
    public float attackRange;
   

    private int positionIndex = 0;
    private Transform _playerToAttack;
    private bool _isFacingRight = false;
    private EnemyState _currentEnemyState = EnemyState.Idle;

    private Animator animator;

    public GameObject bullet;
    public Transform bulletPosition;
    public float shootingRate;
    private float timer;

    public int maxHp;
    private int Hp;


    public Transform PlayerToAttack
    {
        get => _playerToAttack;
        set
        {
            if (value == _playerToAttack) return;
            _playerToAttack = value;
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        set
        {
            if (value == _isFacingRight) return;
            _isFacingRight = value;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public EnemyState CurrentEnemyState
    {
        get => _currentEnemyState;
        set
        {
            if (value == _currentEnemyState) return;
            _currentEnemyState = value;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Hp = maxHp;
    }

    void Update()
    {
        try
        {
            switch (_currentEnemyState)
            {
                case EnemyState.Idle:
                    IdleMovement();
                    break;
                case EnemyState.MoveTowardsPlayer:
                    MoveToPlayer();
                    break;
                case EnemyState.AttacksPlayer:
                    AttackPlayer();
                    break;
            }
        }
        catch (MissingReferenceException exception)
        {
            Debug.Log(exception);
            CurrentEnemyState = EnemyState.Idle;
            animator.SetBool("closeForAttack", false);
        }
        
    }

    private void IdleMovement()
    {
        IsFacingRight = transform.position.x < positions[positionIndex].x;
        transform.position = Vector3.MoveTowards(transform.position, positions[positionIndex], Time.deltaTime * speed);
        if (transform.position == positions[positionIndex])
        {
            if (positionIndex == positions.Length - 1)
            {
                positionIndex = 0;
            }
            else
            {
                positionIndex++;
            }
        }

        if (isSeeingPlayer().Item1)
        {
            CurrentEnemyState = EnemyState.MoveTowardsPlayer;
            PlayerToAttack = isSeeingPlayer().Item2;
        }
    }

    private void MoveToPlayer()
    {
        // if close enough to player, change state to attack
        // else if player is too far, change state to idle
        // else move to player
        float distance = Vector3.Distance(PlayerToAttack.position, transform.position);
        if (distance <= attackRange)
        {
            animator.SetBool("closeForAttack", true);
            CurrentEnemyState = EnemyState.AttacksPlayer;
        }
        else if (distance > playerVisibleRange)
        {
            animator.SetBool("closeForAttack", false);
            CurrentEnemyState = EnemyState.Idle;
        }
        else
        {
            IsFacingRight = transform.position.x < PlayerToAttack.position.x;

            Vector3 positionToMove;
            if (!canFly)
            {
                positionToMove = new Vector3(PlayerToAttack.position.x, transform.position.y);
            } 
            else
            {
                positionToMove = PlayerToAttack.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, positionToMove, Time.deltaTime * (speed + speedIncrease));
        }
    }

    private void AttackPlayer()
    {
        if (PlayerToAttack == null)
        {
            animator.SetBool("closeForAttack", false);
            CurrentEnemyState = EnemyState.Idle;
            return;
        }
        // if player is too far, change state to idle
        // else if player is close, but not close enought for the attack, move towards the player
        // else attack
        float distance = Vector3.Distance(PlayerToAttack.position, transform.position);
        IsFacingRight = transform.position.x < PlayerToAttack.position.x;
        if (distance > playerVisibleRange)
        {
            animator.SetBool("closeForAttack", false);
            CurrentEnemyState = EnemyState.Idle;
        }
        else if (distance > attackRange && distance <= playerVisibleRange)
        {
            animator.SetBool("closeForAttack", false);
            CurrentEnemyState = EnemyState.MoveTowardsPlayer;
        }
    }
    private Tuple<bool, Transform> isSeeingPlayer()
    {
        

        if (player != null)
        {
            if (Vector3.Distance(player.position, transform.position) <= playerVisibleRange)
            {
                return new Tuple<bool, Transform>(true, player);
            }
        }

        return new Tuple<bool, Transform>(false, null);

    }


    public void DroidShoot()
    {
        GameObject bulletShot = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        Vector3 direction = new Vector3(-transform.localScale.x, 0);
        bulletShot.GetComponent<EnemyBulletController>().Setup(direction);

        
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Was attacked!");
        Hp -= damageAmount;
        if (Hp <= 0)
        {
            animator.SetBool("isDisabled", true);
        }
    }
}

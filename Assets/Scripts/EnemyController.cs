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

    public Transform playerOne;
    public Transform playerTwo;
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
    }

    void Update()
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
        else
        {
            Attack(PlayerToAttack);
        }
    }
    private Tuple<bool, Transform> isSeeingPlayer()
    {
        if (Vector3.Distance(playerOne.position, transform.position) <= playerVisibleRange)
            return new Tuple<bool, Transform>(true, playerOne);
        else if (Vector3.Distance(playerTwo.position, transform.position) <= playerVisibleRange)
            return new Tuple<bool, Transform>(true, playerTwo);
        else
            return new Tuple<bool, Transform>(false, null);

    }

    private void Attack(Transform playerToAttack)
    {
        timer += Time.deltaTime;

        if (timer > shootingRate)
        {
            timer = 0;
            shoot(playerToAttack);
        }
    }

    private void shoot(Transform playerToAttack)
    {
        GameObject bulletShot = Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        Vector3 direction = new Vector3(-transform.localScale.x, 0);
        bulletShot.GetComponent<EnemyBulletController>().Setup(direction);

        
    }
}

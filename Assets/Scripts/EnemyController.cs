using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle = 1,
        SeesPlayer = 2,
        AttacksPlayer = 3
    }

    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3[] positions;

    private int positionIndex = 0;
    private bool _isFacingRight = false;

    private EnemyState _currentEnemyState = EnemyState.Idle;

    public Transform playerOne;
    public Transform playerTwo;
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

    void Update()
    {
        switch (_currentEnemyState)
        {
            case EnemyState.Idle:
                IdleMovement();
                break;
            case EnemyState.SeesPlayer:
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

        // if sees player
        // CurrentEnemyState = EnemyState.SeesPlayer;
    }

    private void MoveToPlayer()
    {
        // if close enough to player, CurrentEnemyState = EnemyState.AttacksPlayer; return;
        // else move to player
    }

    private void AttackPlayer()
    {
        // if player is too far CurrentEnemyState = EnemyState.Idle; return;
        // else attack
    }
}

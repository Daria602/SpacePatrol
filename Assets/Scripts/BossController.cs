using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float timeBeforeAttacks;
    private Animator animator;
    public enum BossState
    {
        Idle = 1,
        LightAttack = 2,
        HeavyAttack = 3,
        FallingAttack = 4
    }

    private BossState currentState = BossState.Idle;
    [SerializeField] private Vector3[] fallingAttackPositions;
    public float distanceToTheGround;

    private bool startedFlying = false;
    private bool startedFalling = false;
    Vector3 positionToFlyTo;
    Vector3 positionToFallTo;
    float flyingSpeed = 8;
    float fallingSpeed = 16;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.Idle:
                ChooseAttack();
                break;
            case BossState.LightAttack:
                DoLightAttack();
                break;
            case BossState.HeavyAttack:
                DoHeavyAttack();
                break;
            case BossState.FallingAttack:
                DoFallingAttack();
                break;
        }
    }

    private float timeFromLastAttack = 0;

    private void ChooseAttack()
    {
        // if enough time passed since last attack, choose another attack
        // else increase the timer
        if (timeFromLastAttack >= timeBeforeAttacks)
        {
            // randomly choose the attack here

            int randomAttackIndex = Random.Range(4, 5);
            switch (randomAttackIndex)
            {
                case 2:
                    currentState = BossState.LightAttack;
                    break;
                case 3:
                    currentState = BossState.HeavyAttack;
                    break;
                case 4:
                    currentState = BossState.FallingAttack;
                    break;
            }

            // reset the timer
            timeFromLastAttack = 0;
        } 
        else
        {
            timeFromLastAttack += Time.deltaTime;
        }
    }

    private void DoLightAttack()
    {
        animator.SetTrigger("lightAttack");
    }

    

    private void DoHeavyAttack()
    {
        animator.SetTrigger("heavyAttack");
    }

    
    

    private void DoFallingAttack()
    {
        // fly to one of the positions
        // and then fall down

        // if hasn't started falling, fly to  the position
        // else fall
        if (!startedFalling)
        {
            Fly();
        } 
        else
        {
            Fall();
        }


        
    }

    private void Fly()
    {
        // if hasn't started flying yet, select the position and set startedFlying to true
        if (!startedFlying)
        {
            int positionIndex = Random.Range(0, fallingAttackPositions.Length);
            positionToFlyTo = fallingAttackPositions[positionIndex];
            startedFlying = true;
            animator.SetBool("isFlyingUp", true);
        }
        else
        {
            //Debug.Log(positionToFlyTo);

            transform.position = Vector3.MoveTowards(transform.position, positionToFlyTo, Time.deltaTime * flyingSpeed);

            //transform.position = positionToFlyTo;

            Debug.Log("CurrentPosition");
            Debug.Log(transform.position);
            Debug.Log("GoalPosition");
            Debug.Log(positionToFlyTo);
            

            // check if arrived at the position 
            // start falling
            if (transform.position == positionToFlyTo)
            {
                startedFlying = false;
                startedFalling = true;
                positionToFallTo = new Vector3(transform.position.x, transform.position.y - distanceToTheGround, transform.position.z);
                animator.SetBool("isFlyingUp", false);
                animator.SetBool("isFalling", true);
            }

        }
    }

    private void Fall()
    {
        // fall down

        transform.position = Vector3.MoveTowards(transform.position, positionToFallTo, Time.deltaTime * fallingSpeed);
        
        // if is at the ground, change state to idle, reset startedFalling
        if (transform.position == positionToFallTo)
        {
            startedFalling = false;
            animator.SetBool("isFalling", false);
            currentState = BossState.Idle;
        }
    }

    public void AttackPassed()
    {
        currentState = BossState.Idle;
    }
}

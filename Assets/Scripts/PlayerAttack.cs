using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public Transform attackPosition;
    public float attackRange;
    public LayerMask enemiesMask;
    public int damageAmount;
    private Vector3 positionBeforeAttack;
    public float movementPositionDelay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //positionBeforeAttack = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                //transform.position = new Vector3(transform.position.x + movementPositionDelay, transform.position.y, transform.position.z);
                Debug.Log(positionBeforeAttack);
                Debug.Log(transform.position);
                
                gameObject.GetComponentInChildren<Animator>().SetTrigger("attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemiesMask);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().TakeDamage(damageAmount);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        } else
        {
            timeBtwAttack -= Time.deltaTime;
        }
        
    }

    public void attackOver()
    {
        Debug.Log("Got here");
        
        //transform.position = new Vector3(positionBeforeAttack.x, positionBeforeAttack.y, positionBeforeAttack.z);
        Debug.Log(transform.position);
    }
}

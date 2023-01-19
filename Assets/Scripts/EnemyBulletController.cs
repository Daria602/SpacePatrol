using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    
    public float bulletSpeed;
    private Vector3 direction;
    public LayerMask playerMask;
    private float timer;

    private void Awake()
    {
        timer = 0;
    }



    void Update()
    {
        timer += Time.deltaTime;

        // If bullet is alive for 5 deltaTimes, destroy it
        if (timer > 5)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(direction * bulletSpeed * Time.deltaTime);

        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, 0.1f, playerMask);


        if (hitPlayer.name == "Player")
        {
            hitPlayer.GetComponent<PlayerController>().shouldTP = false;
            hitPlayer.GetComponent<DamageReceiver>().TakeDamage(-1);
            Destroy(gameObject);
            return;
        } 

        if (hitPlayer.name == "FloorCeiling")
        {
            Destroy(gameObject);
            return;
        }

        

    }

    public void Setup(Vector3 direction)
    {
        this.direction = direction;
    }

    public void BulletDestroyed()
    {
        Destroy(gameObject);
    }

}

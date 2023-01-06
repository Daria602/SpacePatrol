using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    
    public float bulletSpeed;
    private Vector3 direction;
    RaycastHit2D hit;
    private float playerHitDistance = 1;



    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);

        hit = Physics2D.Raycast(transform.position, transform.position, playerHitDistance);
        if (hit.collider != null)
        {
            gameObject.GetComponent<Animator>().SetBool("hasCollided", true);
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

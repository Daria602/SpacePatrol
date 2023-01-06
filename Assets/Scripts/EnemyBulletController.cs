using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    
    public float bulletSpeed;
    private Vector3 direction;

   


    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    public void Setup(Vector3 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Got here");
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Vector3 _direction;
    public float bulletSpeed;

    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }
    void Start()
    {
        _direction = transform.localScale;
    }


    void Update()
    {
        //Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, Vector3.left, Time.deltaTime*bulletSpeed);
    }


}

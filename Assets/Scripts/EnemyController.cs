using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3[] positions;

    private int positionIndex = 0;
    private bool _isFacingRight = false;

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

    void Update()
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
    }
}

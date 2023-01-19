using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    // Positions to move to in Idle state
    [SerializeField] public Vector3[] positions;
    private int positionIndex = 0;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IdleMovement();
    }

    private void IdleMovement()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, positions[positionIndex], Time.deltaTime * speed);
        if (transform.localPosition == positions[positionIndex])
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

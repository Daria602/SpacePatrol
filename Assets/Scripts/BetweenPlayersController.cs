using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetweenPlayersController : MonoBehaviour
{
    public Transform playerOneTarget;
    public Transform playerTwoTarget;

    private void Awake()
    {

    }
    void Start()
    {
        
    }

    void Update()
    {
        if (playerOneTarget == null)
        {
            transform.position = new Vector2(playerTwoTarget.position.x, transform.position.y);
        } 
        else if (playerTwoTarget == null)
        {
            transform.position = new Vector2(playerOneTarget.position.x, transform.position.y);
        }
        else if (playerOneTarget != null && playerTwoTarget != null) 
        {
            transform.position = new Vector2((playerOneTarget.position.x + playerTwoTarget.position.x) / 2, transform.position.y);
        } else
        {
            return;
        }
    }
}

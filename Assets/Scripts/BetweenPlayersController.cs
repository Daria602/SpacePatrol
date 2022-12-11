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
        transform.position = new Vector2((playerOneTarget.position.x + playerTwoTarget.position.x) / 2, transform.position.y);
    }
}

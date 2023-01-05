using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    public Transform player;
    public string playerHorizAxis;
    public string playerJumpButton;
    private bool shouldBeShown;
    // Start is called before the first frame update
    void Start()
    {
        shouldBeShown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeShown)
        {
            transform.position = new Vector3(player.position.x + 0.3f, player.position.y + 1.5f, player.position.z);

            // if moved
            if (Input.GetAxis(playerHorizAxis) != 0 || Input.GetButtonDown(playerJumpButton))
            {
                shouldBeShown = false;
                gameObject.SetActive(false);
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorController : MonoBehaviour
{
    // After the player finished the level, both of them will be teleported
    public Vector3 teleportPlayer;
    public GameObject player;


    private bool playerEntered = false;

    void Update()
    {
        if (playerEntered)
        {
            player.transform.localPosition = teleportPlayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.name == "Player")
            playerEntered = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.name == "Player")
            playerEntered = false;
    }
}

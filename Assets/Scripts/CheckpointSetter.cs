using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSetter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.name == "PlayerOne" || collision.gameObject.name == "PlayerTwo")
            collision.gameObject.GetComponent<PlayerController>().lastCheckpoint = this.gameObject.transform.localPosition;
    }
}
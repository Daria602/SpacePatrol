using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "PlayerOne" || collision.name == "PlayerTwo")
        {
            gameObject.SetActive(false);
            //Call a function in PlayerOneController to do something...
        }
    }
}

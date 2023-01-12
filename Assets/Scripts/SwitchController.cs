using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Sprite active;
    public Sprite deactivated;
    private bool isDeactivated = false;
    public List<GameObject> enemiesToDisable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDeactivated = !isDeactivated;
        }
        if (isDeactivated)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = deactivated;
        } else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = active;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerOne" || collision.gameObject.name == "PlayerTwo")
        {
            isDeactivated = true;

            for (int i = 0; i < enemiesToDisable.Count; i++)
            {
                enemiesToDisable[i].GetComponent<Animator>().SetBool("isDisabled", true);
            }
        }
    }
}
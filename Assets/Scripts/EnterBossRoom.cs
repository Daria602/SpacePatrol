using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBossRoom : MonoBehaviour
{
    public GameObject bossObject;
    private bool canBeActivated = true;
    void Start()
    {
        bossObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision == null) return;

        if (collision.gameObject.GetComponent<DamageReceiver>() is DamageReceiver damageReceiver)
        { 
            if (canBeActivated)
            {
                bossObject.SetActive(true);
                canBeActivated = false;
            }
            
        }
    }

    
}

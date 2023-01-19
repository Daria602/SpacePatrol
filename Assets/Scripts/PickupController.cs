using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum PickupType
    {
        SmallValue = 10,
        BigValue = 100
    }

    [SerializeField] public PickupType pickupType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.name == "Player")
        {
            
            switch (pickupType)
            {
                case PickupType.SmallValue:
                    // do something with generic
                    FindObjectOfType<AudioManager>().PlayPickup();
                    FindObjectOfType<GameManager>().AddToScore((int)PickupType.SmallValue);
                    break;
                case PickupType.BigValue:
                    // do something with generic
                    FindObjectOfType<AudioManager>().PlayBigPickup();
                    FindObjectOfType<GameManager>().AddToScore((int)PickupType.SmallValue);
                    break;
            }

            gameObject.SetActive(false);
        }
        
    }
}

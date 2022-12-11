using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public enum PickupType
    {
        Generic = 0,
        Medkit = 1
    }

    [SerializeField] public PickupType pickupType;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        switch (pickupType)
        {
            case PickupType.Generic:
                // do something with generic
                break;
            case PickupType.Medkit:
                // do something with medkit
                break;
        }
    }
}

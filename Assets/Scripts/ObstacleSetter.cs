using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSetter : MonoBehaviour
{

    public bool TeleportOnDamage;
    public int HealthModifier;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<DamageReceiver>() is DamageReceiver damageReceiver)
        {
            collision.gameObject.GetComponent<PlayerController>().shouldTP = TeleportOnDamage;
            damageReceiver.TakeDamage(HealthModifier);
        }
    }
}

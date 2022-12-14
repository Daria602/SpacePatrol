using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnTrigger : MonoBehaviour
{
    /// <summary>
    /// Can be a positive or a negative amount (positive substructs from health, negative adds)
    /// </summary>
    public int DamageToDeal;
    public bool TeleportOnDamage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.gameObject.GetComponent<DamageReceiver>() is DamageReceiver damageReceiver)
        {
            collision.gameObject.GetComponent<PlayerController>().shouldTP = TeleportOnDamage;
            damageReceiver.TakeDamage(DamageToDeal);
        }
    }
}

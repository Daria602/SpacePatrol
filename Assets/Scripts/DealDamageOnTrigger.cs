using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnTrigger : MonoBehaviour
{
    /// <summary>
    /// Can be a positive or a negative amount
    /// </summary>
    public int DamageToDeal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if(collision.gameObject.GetComponent<DamageReceiver>() is DamageReceiver damageReceiver)
        {
            damageReceiver.TakeDamage(DamageToDeal);
        }
    }
}

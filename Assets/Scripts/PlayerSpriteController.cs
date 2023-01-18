using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public void hurtAnimationPassed()
    {
        gameObject.GetComponentInParent<PlayerController>().hurtAnimationPassed();
    }

    public void DeathAnimationPassed()
    {
        gameObject.GetComponentInParent<PlayerController>().DeathAnimationPassed();

    }

    public void attackOver()
    {
        gameObject.GetComponentInParent<PlayerController>().AttackOver();
    }
}

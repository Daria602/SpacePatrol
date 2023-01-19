using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource bigPickupSound;

    public void PlayPickup()
    {
        pickupSound.Play();
    }
    public void PlayBigPickup()
    {
        bigPickupSound.Play();
    }
}

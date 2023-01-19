using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource pickupSound;
    
    public void PlayPickup()
    {
        pickupSound.Play();
    }
}

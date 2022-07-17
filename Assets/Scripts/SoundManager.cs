using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound")]
    private AudioSource audioSource;
    public PlayerController player;

    [Header("PlayerSound")]
    public AudioClip shoot;
    public AudioClip Reload;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SoundSetup(string soundCilp)
    {
        switch (soundCilp)
        {
            case "shoot":
                audioSource.clip = shoot;
                audioSource.Play();
                break;
            case "Reloadsound":
                player.audiosource.clip = Reload;
                player.audiosource.Play();
                break;
        }
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class SoundManager : NetworkBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip[] backGroundMusic;

    [SerializeField] bool isMainMenu;

    void Start()
    {
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        if (isMainMenu)
        {
            audioSource.clip = backGroundMusic[0];
        }
        else
        {
            audioSource.clip = backGroundMusic[Random.Range(1, backGroundMusic.Length - 1)];
            Debug.Log(backGroundMusic.Length);
        }
        audioSource.Play();
    }


}
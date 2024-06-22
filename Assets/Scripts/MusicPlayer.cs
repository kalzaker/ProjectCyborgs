using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    [SerializeField] public AudioSource audioSource;

    [SerializeField] AudioClip[] backGroundMusic;

    int randomValue;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextTrack();
        }
    }

    public void PlayNextTrack()
    {
        randomValue = Random.Range(0, backGroundMusic.Length);
        Debug.Log(randomValue);
        audioSource.clip = backGroundMusic[randomValue];
        Debug.Log(audioSource.clip);
        audioSource.Play();
    }
}

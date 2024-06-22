using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class SoundManager : NetworkBehaviour
{
    public static SoundManager instance;

    [SerializeField] public AudioSource audioSource;

    [SerializeField] bool isMainMenu;

    public float musicVolume = 0.3f;
    public float sfxVolume = 0.3f;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicVolume;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        GetComponent<MusicPlayer>().PlayNextTrack();
    }
}
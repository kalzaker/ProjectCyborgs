using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    private List<AudioSource> audioSources;

    public static float soundVolume = 0.3f;

    void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }
        audioSources = new List<AudioSource>(GetComponents<AudioSource>());
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource availableSource = GetAvailableAudioSource();
        if (availableSource != null)
        {
            availableSource.clip = clip;
            availableSource.Play();
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return null;
    }
}
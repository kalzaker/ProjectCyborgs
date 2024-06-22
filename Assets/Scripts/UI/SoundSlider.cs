using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] bool isSfxSlider;

    void Start()
    {
        if (isSfxSlider) 
        { 
            this.GetComponent<Slider>().value = SoundManager.instance.sfxVolume;
        }

        else
        {
            this.GetComponent<Slider>().value = SoundManager.instance.musicVolume;
        }
    }

    public void SetSFXVolume()
    {
        SoundManager.instance.sfxVolume = this.GetComponent<Slider>().value;
    }

    public void SetMusicVolume()
    {
        SoundManager.instance.musicVolume = this.GetComponent<Slider>().value;
        SoundManager.instance.audioSource.volume = SoundManager.instance.musicVolume;
    }
}
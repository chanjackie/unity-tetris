using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MenuSettings : MonoBehaviour
{
    public AudioMixer bgmAudioMixer;
    public AudioMixer effectsAudioMixer;

    public void SetBGMVolume(float volume) {
        if (volume <= -30) {
            volume = -80;
        }
        bgmAudioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetEffectsVolume(float volume) {
        if (volume <= -30) {
            volume = -80;
        }
        effectsAudioMixer.SetFloat("EffectsVolume", volume);
    }
}

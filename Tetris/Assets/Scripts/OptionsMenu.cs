using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider effectsSlider;
    public AudioMixer masterAudioMixer;
    void Start()
    {   
        float bgmVolume;
        float effectsVolume;
        masterAudioMixer.GetFloat("BGMVolume", out bgmVolume);
        masterAudioMixer.GetFloat("EffectsVolume", out effectsVolume);
        // Set slider values by inversing log
        bgmVolume = Mathf.Pow(10, (bgmVolume/20.0f));
        effectsVolume = Mathf.Pow(10, (effectsVolume/20.0f));
        bgmSlider.SetValueWithoutNotify(bgmVolume);
        effectsSlider.SetValueWithoutNotify(effectsVolume);
    }

    public void SetBGMVolume(float volume) {
        // Avoid log 0
        if (volume <= 0) {
            volume = 0.0001f;
        }
        masterAudioMixer.SetFloat("BGMVolume", Mathf.Log10(volume)*20);
    }

    public void SetEffectsVolume(float volume) {
        // Avoid log 0
        if (volume <= 0) {
            volume = 0.0001f;
        }
        masterAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume)*20);
    }
}

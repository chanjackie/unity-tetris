using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider effectsSlider;
    public AudioMixer bgmMixer;
    public AudioMixer effectsMixer;
    void Start()
    {   
        float bgmVolume;
        float effectsVolume;
        bgmMixer.GetFloat("BGMVolume", out bgmVolume);
        effectsMixer.GetFloat("EffectsVolume", out effectsVolume);
        bgmSlider.SetValueWithoutNotify(bgmVolume);
        effectsSlider.SetValueWithoutNotify(effectsVolume);
    }
}

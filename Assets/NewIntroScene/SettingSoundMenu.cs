using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingSoundMenu : MonoBehaviour
{
    public Slider masterAudioSlider;
    public Slider SFXAudioSlider;
    public Slider BGMAudioSlider;

    void Start()
    {
        SoundManager.instance.audioMixer.GetFloat("Master", out float m1);
        SoundManager.instance.audioMixer.GetFloat("SFX", out float m2);
        SoundManager.instance.audioMixer.GetFloat("BGM", out float m3);
        masterAudioSlider.value = m1 > -80f ? (m1 + 20) / 40 : -80f;
        SFXAudioSlider.value = m2 > -80f ? (m2 + 20) / 40 : -80f;
        BGMAudioSlider.value = m3 > -80f ? (m3 + 20) / 40 : -80f;
        masterAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("Master", val));
        SFXAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("SFX", val));
        BGMAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("BGM", val));
    }

    private void OnSoundSliderValChanged(string name, float val)
    {
        float v = val > 0.01f ? (val * 40) - 20 : -80f;
        SoundManager.instance.audioMixer.SetFloat(name, v);
        Debug.Log($"{name} : {v}");
    }
}

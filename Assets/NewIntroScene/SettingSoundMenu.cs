using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingSoundMenu : MonoBehaviour
{
    public Slider masterAudioSlider; // 마스터 볼륨 슬라이더
    public Slider SFXAudioSlider; // 효과음 슬라이더
    public Slider BGMAudioSlider; // 배경음 슬라이더

    void Start()
    {
        SoundManager.instance.audioMixer.GetFloat("Master", out float m1);
        SoundManager.instance.audioMixer.GetFloat("SFX", out float m2);
        SoundManager.instance.audioMixer.GetFloat("BGM", out float m3);
        masterAudioSlider.value = (m1 + 80) / 100;
        SFXAudioSlider.value = (m2 + 80) / 100;
        BGMAudioSlider.value = (m3 + 80) / 100;
        masterAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("Master", val));
        SFXAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("SFX", val));
        BGMAudioSlider.onValueChanged.AddListener(val => OnSoundSliderValChanged("BGM", val));
    }

    private void OnSoundSliderValChanged(string name, float val)
    {
        SoundManager.instance.audioMixer.SetFloat(name, val * 100 - 80);
        Debug.Log($"{name} : {val * 100 - 80}");
    }
}

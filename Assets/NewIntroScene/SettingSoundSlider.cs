using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingSoundSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(SoundSliderValChanged);
    }

    private void SoundSliderValChanged(float val)
    {
        Debug.Log(val);
        AudioListener.volume = val;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioGroupSlider : MonoBehaviour {

    public AudioMixerGroup amg;
    public string floatToSet;

    private void Start()
    {
        Slider s = GetComponent<Slider>();
        float f = 0;

        if (amg.audioMixer.GetFloat(floatToSet, out f))
        {
            s.value = f;
        }

    }
    public void ChangeVolume(Slider slider)
    {
        amg.audioMixer.SetFloat(floatToSet, slider.value);
    }
}

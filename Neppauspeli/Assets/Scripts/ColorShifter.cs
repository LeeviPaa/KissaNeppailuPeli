using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class ColorShifter : MonoBehaviour {

    public PostProcessingProfile ppp;
    private float frames = 0;
    public float speed = 1;

    private void Start()
    {
        if (ppp != null)
        {
            ColorGradingModel.Settings cs = ppp.colorGrading.settings;
            cs.basic.hueShift = 0;
            ppp.colorGrading.settings = cs;
        }
    }
    private void Update()
    {
        if (ppp != null)
        {

            frames = frames + speed * Time.deltaTime;

            if (frames >= 180)
                frames = -180;

            ColorGradingModel.Settings cs = ppp.colorGrading.settings;
            cs.basic.hueShift = frames;
            ppp.colorGrading.settings = cs;
        }
    }
    private void OnDisable()
    {
        if (ppp != null)
        {
            ColorGradingModel.Settings cs = ppp.colorGrading.settings;
            cs.basic.hueShift = 0;
            ppp.colorGrading.settings = cs;
        }
    }
}

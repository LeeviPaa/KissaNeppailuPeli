using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreen : MonoBehaviour {

    Toggle t;
    private void Start()
    {
        t = GetComponent<Toggle>();
        t.isOn = Screen.fullScreen;
    }

    public void ToggleFullscreenFunction(Toggle toggle)
    {
        Resolution r = Screen.currentResolution;
        Screen.SetResolution(r.width, r.height, toggle.isOn);
    }
}

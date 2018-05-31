using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettigsUI : MonoBehaviour {

    string[] names;

    private void Start()
    {
        names = QualitySettings.names;
    }
    public void SetQualitySettings(Dropdown d)
    {
        int i = d.value;
        if (i < 0 || i >= names.Length)
            i = 0;

        QualitySettings.SetQualityLevel(i);
    }
}

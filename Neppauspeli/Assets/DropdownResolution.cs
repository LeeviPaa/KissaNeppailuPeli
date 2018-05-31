using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownResolution : MonoBehaviour {

    Dropdown dd;
    Resolution currRes;
    List< Dropdown.OptionData> optionData = new List<Dropdown.OptionData>();
    Resolution[] resolist;
    int defIndex = 0;

    private void Start()
    {
        dd = GetComponent<Dropdown>();
        resolist = Screen.resolutions;
        foreach(Resolution r in resolist)
        {
            Dropdown.OptionData d = new Dropdown.OptionData(r.ToString());
            optionData.Add(d);

            if(Screen.currentResolution.ToString() == r.ToString())
            {
                defIndex = optionData.Count - 1;
            }
        }
        dd.options = optionData;
        
        dd.value = defIndex;
    }
    public void OnValueChange()
    {
        int indx = dd.value;
        if (indx < resolist.Length)
        {
            Debug.Log(resolist[indx].ToString());
            Resolution r = resolist[indx];
            Screen.SetResolution(r.width, r.height, Screen.fullScreen);
        }
    }
    public void ToggleFullscreen(Toggle toggle)
    {
        Screen.fullScreen = toggle.isOn;
    }
}

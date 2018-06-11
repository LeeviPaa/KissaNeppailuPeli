using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventAudio : MonoBehaviour, IMoveHandler, ISelectHandler, ISubmitHandler
{

    public AudioClip moveMenu;
    public AudioClip selectMenu;
    public EventSystem es;

    private void Start()
    {
        
        
    }
    public void OnMove(AxisEventData aed)
    {
        Debug.Log("Move");
    }
    public void OnSelect(BaseEventData bed)
    {
        print("Select");
    }
    public void OnSubmit(BaseEventData bed)
    {
        print("Submit");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : MonoBehaviour {

    public int index = 0;
    public bool locked = true;

    public void Unlock()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        locked = false;
    }
}

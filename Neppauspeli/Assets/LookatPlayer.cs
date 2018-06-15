using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayer : MonoBehaviour {

    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }

    void Update () {
        if (cam != null)
        {
            transform.LookAt(cam.transform);
            transform.Rotate(0, 180, 0);
        }

	}
}

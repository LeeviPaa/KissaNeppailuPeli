using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollbarscroll : MonoBehaviour {

    public Scrollbar scroll;
    public float speed = 10;

    private void Update()
    {
        float input = Input.GetAxis("Vertical4");
        if (input > 0.1f || input < 0.1f)
        {
            scroll.value += input * Time.deltaTime * speed;
        }
    }
}

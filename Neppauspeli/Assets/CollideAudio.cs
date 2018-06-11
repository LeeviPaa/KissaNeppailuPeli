using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideAudio : MonoBehaviour {

    public AudioClip hit1;
    public AudioClip hit2;
    public AudioSource ac;

    public void OnCollisionEnter(Collision collision)
    {
        if (ac != null)
        {
            int i = UnityEngine.Random.Range(0, 1);

            if (i == 0)
            {
                ac.PlayOneShot(hit1);
            }
            else
            {
                ac.PlayOneShot(hit2);
            }
        }
    }
}

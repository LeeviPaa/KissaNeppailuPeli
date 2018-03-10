using UnityEngine;
using System.Collections;

public class PlayerCollisionSound : MonoBehaviour {

    public AudioClip collClip;
    private AudioSource currSource;
    private float CollIntens;
    void Start()
    {
        currSource = gameObject.GetComponent<AudioSource>();
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "CollCube")
        {
            CollIntens = col.relativeVelocity.magnitude / 20;
            currSource.PlayOneShot(collClip, CollIntens);
        }
    }
}

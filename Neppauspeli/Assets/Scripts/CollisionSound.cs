using UnityEngine;
using System.Collections;

public class CollisionSound : MonoBehaviour {

    float CollisionIntensity;
    private AudioSource ausour;
    private AudioClip auclip;
    private Holderscript GetStuff;
    void Start()
    {
        GetStuff = GameObject.FindGameObjectWithTag("GameController").GetComponent<Holderscript>();
        auclip = GetStuff.getaudioclip();
        ausour = gameObject.AddComponent<AudioSource>();
        ausour.volume = 0.25f;
    }
    void OnCollisionEnter(Collision col)
    {
        CollisionIntensity = col.relativeVelocity.magnitude/20;
        ausour.PlayOneShot(auclip, CollisionIntensity);
        
        Debug.Log(CollisionIntensity);
    }
}

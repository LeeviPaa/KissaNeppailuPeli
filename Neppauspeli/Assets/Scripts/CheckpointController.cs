using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

    public int currentCheckpoint;
    private bool triggered = false;
    public GameObject TriggerEffects;
    public GameObject TheLamp;
    public Material NonEmissive;
    public Material Emissive;
    public AudioSource ac;
    public AudioClip checkpointClip;

    private void Start()
    {
        if(TheLamp != null && NonEmissive != null)
        {
            TheLamp.GetComponent<MeshRenderer>().material = NonEmissive;
        }

        if (TriggerEffects != null)
        {
            TriggerEffects.SetActive(false);
        }
        else
            Debug.LogError(this.name + " trigger effects not found");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Playercheckpoint>() && !triggered)
        {
            other.GetComponent<Playercheckpoint>().SetCurrentCheckpoint(transform);
            Debug.Log(other);
            if(TriggerEffects != null)
            {
                TriggerEffects.SetActive(true);
            }
            if (TheLamp != null && Emissive != null)
            {
                TheLamp.GetComponent<MeshRenderer>().material = Emissive;
            }
            if(ac != null)
            {
                ac.PlayOneShot(checkpointClip);
            }
            triggered = true;
        }
    }
}

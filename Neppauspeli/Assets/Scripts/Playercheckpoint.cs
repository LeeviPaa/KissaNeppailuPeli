using UnityEngine;
using System.Collections;

public class Playercheckpoint : MonoBehaviour {

    private Transform currCheckpoint;
    void Start()
    {
        currCheckpoint = transform;
    }
    public void SetCurrentCheckpoint(Transform setted)
    {
        currCheckpoint = setted;
    }
    public void SetPosition()
    {
        transform.position = currCheckpoint.position;
        transform.rotation = currCheckpoint.rotation;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}

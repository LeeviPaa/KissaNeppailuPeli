using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour {

    public Vector3 customGravity = Vector3.up;
    private List<Rigidbody> rList = new List<Rigidbody>();

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in rList)
        {
            if(rb != null)
            {
                rb.AddForce(customGravity);
            }
            else
            {
                rList.Remove(rb);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && !rList.Contains(rb))
        {
            rList.Add(rb);
            rb.useGravity = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null && rList.Contains(rb))
        {
            rList.Remove(rb);
            rb.useGravity = true;
        }
    }
}

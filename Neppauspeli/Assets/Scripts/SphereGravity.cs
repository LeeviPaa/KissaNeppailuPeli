using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGravity : MonoBehaviour {

    public float customGravityAmount = 10;
    private List<Rigidbody> rList = new List<Rigidbody>();

    private void FixedUpdate()
    {
        foreach(Rigidbody rb in rList)
        {
            if(rb != null)
            {
                Vector3 customGravity = (transform.position - rb.transform.position).normalized*customGravityAmount;
                rb.AddForce(customGravity+Vector3.up);
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

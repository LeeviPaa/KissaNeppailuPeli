using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour {

    EventManager em;
    private void Awake()
    {
        em = Toolbox.RegisterComponent<EventManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            em.BroadcastPlayerDied();
        }
    }
}

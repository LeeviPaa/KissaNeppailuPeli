using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKiller : MonoBehaviour {

    EventManager em;
    private void Awake()
    {
        em = Toolbox.RegisterComponent<EventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            em.BroadcastPlayerDied();
        }
    }
}

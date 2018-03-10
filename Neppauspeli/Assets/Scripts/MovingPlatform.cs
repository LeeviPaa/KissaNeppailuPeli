using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    Transform playerParent;

    private void OnTriggerEnter(Collider other)
    {
        player_movement pm = other.gameObject.GetComponent<player_movement>();
        if(pm != null)
        {
            playerParent = other.transform.parent;
            other.transform.parent = transform.parent;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        player_movement pm = other.gameObject.GetComponent<player_movement>();
        if (pm != null)
        {
            other.transform.parent = null;
        }
    }
}

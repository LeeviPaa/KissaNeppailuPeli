using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Goal : MonoBehaviour
    {
    private bool checkinput = false;
    // Use this for initialization

    private EventManager EM;
    private LevelInstance LI;
	void Start ()
    {
        LI = Toolbox.RegisterComponent<LevelInstance>();
        EM = Toolbox.RegisterComponent<EventManager>();
	}
	void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LI.GameComplete();
            EM.BroadcastGamePaused();
            checkinput = true;
            if(other.gameObject.GetComponent<Rigidbody>())
            {
                other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}

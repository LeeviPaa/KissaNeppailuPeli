using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour {

    private GameObject mesh;
    private GameObject particles;
    private bool triggered = false;
    private GameObject audio;

    private EventManager EM;
    private LevelInstance LI;

	void Start () {
        LI = Toolbox.RegisterComponent<LevelInstance>();
        EM = Toolbox.RegisterComponent<EventManager>();
        bool particlesFound = false, meshFound = false, audioFound = false;
        foreach(Transform child in transform)
        {
            if(child.name == "Mesh")
            {
                mesh = child.gameObject;
                meshFound = true;
                mesh.SetActive(true);
            }
            if(child.name == "Particles")
            {
                particles = child.gameObject;
                particlesFound = true;
                particles.SetActive(false);
            }
            if(child.name == "SoundFX")
            {
                audio = child.gameObject;
                audioFound = true;
                audio.SetActive(false);
            }
        }
        if (!particlesFound)
            Debug.LogError(name + " particles not found!");
        if (!meshFound)
            Debug.LogError(name + " mesh not found!");
        if (!audioFound)
            Debug.LogError(name + " audio not found!");

	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !triggered)
        {
            triggered = true;
            mesh.SetActive(false);
            particles.SetActive(true);
            audio.SetActive(true);
            LI.TokenIncrement();
        }
    }
}

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class player_movement : MonoBehaviour {

    public float rotationSpeed = 0.5f;
    public float forceMulti = 2.0f;

    private Rigidbody thisrigid;

    private RaycastHit hit;
    private float playersize = 0.5f;
    public float normalDrag = 0.1f;
    public float breakingDrag = 0.5f;
    public GameObject brakingVisuals;
    public GameObject deathVisuals;
    public GameObject normalVisuals;

    public AudioClip nep1;
    public AudioClip nep2;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip catDie;
    public AudioSource ac;

    private bool GameGoing = true;
    private bool GameComplete = false;

    private EventManager EM;
    private void OnEnable()
    {
        if (deathVisuals != null)
            deathVisuals.SetActive(false);
        if (normalVisuals != null)
            normalVisuals.SetActive(true);
        EM = Toolbox.RegisterComponent<EventManager>();
        EM.gamePaused += ListenGamePaused;
        EM.gameUnPaused += ListenGameUnPaused;
        EM.GameComplete += ListenGameComplete;
        EM.PlayerRespawn += Respawn;
        EM.PlayerDied += Die;
        brakingVisuals.SetActive(false);
    }
    private void OnDisable()
    {
        EM.gamePaused -= ListenGamePaused;
        EM.gameUnPaused -= ListenGameUnPaused;
        EM.GameComplete -= ListenGameComplete;
        EM.PlayerRespawn -= Respawn;
        EM.PlayerDied -= Die;
    }
    void ListenGameComplete()
    {
        GameComplete = true;
    }
    private void ListenGamePaused()
    {
        GameGoing = false;
    }
    private void ListenGameUnPaused()
    {
        GameGoing = true;
    }

    void Start()
    {
        thisrigid = transform.GetComponent<Rigidbody>();
        if(thisrigid)
        {
            thisrigid.drag = normalDrag;
        }
    }
	void Update ()
    {
        if (GameGoing && !GameComplete)
        {
            returnRot();
            Breaking();
        }

    }
    void ConsoleOutput()
    {
        //Debug.Log(transform.forward);
        //Debug.Log(thisrigid.velocity);
    }

    void Breaking()
    {
        if(Input.GetButton("Fire1") && thisrigid != null)
        {
            thisrigid.drag = breakingDrag;
            if(brakingVisuals && !brakingVisuals.activeSelf)
            {
                brakingVisuals.SetActive(true);
            }
        }
        else
        {
            if(thisrigid != null)
            {
                thisrigid.drag = normalDrag;

            }
            if (brakingVisuals && brakingVisuals.activeSelf)
            {
                brakingVisuals.SetActive(false);
            }
        }
    }

    void returnRot()
    {
        if (Input.GetButtonDown("Reset"))
        {
            transform.GetComponent<Playercheckpoint>().SetPosition();
            EM.BroadcastPlayerRespawn();
        }
    }
    public void applyForce(Vector3 forceVector, float distanceSpeed)
    {
        thisrigid.AddForce((transform.forward + forceVector)*-distanceSpeed);
        int i = UnityEngine.Random.Range(0, 1);

        if(i == 0)
        {
            ac.PlayOneShot(nep1);
        }
        else
        {
            ac.PlayOneShot(nep2);
        }

    }
    public void Die()
    {
        thisrigid.isKinematic = true;

        if(deathVisuals != null)
            deathVisuals.SetActive(true);
        if(normalVisuals != null)
            normalVisuals.SetActive(false);

        if (ac != null)
            ac.PlayOneShot(catDie);
    }
    public void Respawn()
    {
        thisrigid.isKinematic = false;

        if (deathVisuals != null)
            deathVisuals.SetActive(false);
        if (normalVisuals != null)
            normalVisuals.SetActive(true);
    }
    bool timerOn = false;
    public void OnCollisionEnter(Collision collision)
    {
        if (ac != null && !timerOn && collision.relativeVelocity.magnitude > 1)
        {
            int i = UnityEngine.Random.Range(0, 1);

            if (i == 0)
            {
                ac.PlayOneShot(hit1, Mathf.Clamp(collision.relativeVelocity.magnitude/5, 0, 1));
            }
            else
            {
                ac.PlayOneShot(hit2, Mathf.Clamp(collision.relativeVelocity.magnitude/5, 0, 1));
            }
            StartCoroutine(ITimer());
        }
    }
    IEnumerator ITimer()
    {
        timerOn = true;
        yield return new WaitForSeconds(0.15f);
        timerOn = false;
    }
}

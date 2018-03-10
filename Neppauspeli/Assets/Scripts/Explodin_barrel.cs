using UnityEngine;
using System.Collections;

public class Explodin_barrel : MonoBehaviour {
    public float ExplosForce = 500;
    public float ExplosRadius = 15;
    public Transform explosionpoint;
    public GameObject Effect;
    private GameObject player;
    private Rigidbody playerRigid;
    public AudioClip explosionsound;
    private AudioSource source;

    private bool triggered = false;

	void Start ()
    {
        source = transform.gameObject.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        if(Effect)
        {
            Effect.gameObject.SetActive(false);
        }
	}
	
	void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            source.PlayOneShot(explosionsound);
            triggered = true;
        }
        Effect.gameObject.SetActive(true);
        transform.GetComponent<MeshRenderer>().enabled = false;
        playerRigid = other.transform.GetComponentInParent<Rigidbody>();
        playerRigid.AddExplosionForce(ExplosForce, explosionpoint.transform.position, ExplosRadius);
        Debug.Log("BOOOOM!");
        StartCoroutine(deathtimer());
    }
    IEnumerator deathtimer()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}

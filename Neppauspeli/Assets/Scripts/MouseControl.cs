using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseControl : MonoBehaviour
{
    private float dragDistance;
    private float dragDistance2;
    private Vector2 mouseStart;
    private Vector2 MouseStop;
    private Vector2 CurrMousePos;
    private Vector2 CurrDirVector;
    private Vector2 LocalForceVector;
    private float upForce = 0;
    public float upForceDelta = 1;
    public float forceAmount = 20;
    public float MaxMinUpForce = 1;
    public GameObject Lookat;
    public GameObject DragIndicator;
    private Text PokeText;
    private player_movement Playerscript;
    private GameObject player;
    private bool dragging = false;
    private int FlickCounter = 0;

    private bool GameGoing = true;
    private bool GameComplete = false;
    private bool playerAlive = true;

    private EventManager EM;
    private LevelInstance LI;
    private void OnEnable()
    {
        LI = Toolbox.RegisterComponent<LevelInstance>();
        EM = Toolbox.RegisterComponent<EventManager>();
        EM.gamePaused += RecieveGamePaused;
        EM.gameUnPaused += RecieveGameUnPaused;
        EM.GameComplete += RecieveGameComplete;
        EM.PlayerRespawn += Respawn;
        EM.PlayerDied += Die;
    }
   
    void RecieveGameComplete()
    {
        GameComplete = true;
    }
    private void OnDisable()
    {
        EM.gamePaused -= RecieveGamePaused;
        EM.gameUnPaused -= RecieveGameUnPaused;
        EM.GameComplete -= RecieveGameComplete;
        EM.PlayerRespawn -= Respawn;
        EM.PlayerDied -= Die;
    }

    public void RecieveGamePaused()
    {
        GameGoing = false;
    }
    public void RecieveGameUnPaused()
    {
        GameGoing = true;
    }

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Playerscript = player.GetComponent<player_movement>();
	}
	

	void Update ()
    {
        if (GameGoing && !GameComplete && playerAlive)
        {
            CurrMousePos = Input.mousePosition;
            MouseInput();
            MouseRot();
        }
	}
    void MouseInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            dragging = true;
            mouseStart = Input.mousePosition;
            upForce = 0;
        }
        if(Input.GetMouseButtonUp(0))
        {
            FlickCounter++;
            LI.NeppausIncrement();
            dragging = false;
            MouseStop = Input.mousePosition;
            mouseStart = Vector2.zero;
            Lookat.transform.position = player.transform.forward;
            DragDir();
        }
    }
    void MouseRot()
    {
        CurrDirVector = CurrMousePos - new Vector2(Screen.width / 2, Screen.height / 2);
        dragDistance2 = Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(CurrDirVector.x, 2) + Mathf.Pow(CurrDirVector.y*1.7f, 2)), 0, 400);
        Vector3 to = new Vector3(CurrDirVector.x/10,0,CurrDirVector.y/10);
        Lookat.transform.localPosition = to;

        if (dragging)
        {
            if (Input.GetAxis("Vertical") < -0.1f || Input.GetAxis("Vertical") > 0.1f)
            {
                upForce += Input.GetAxis("Vertical") * upForceDelta * Time.deltaTime;
                upForce = Mathf.Clamp(upForce, -2, 2);
                DragIndicator.transform.localRotation = Quaternion.Euler(upForce*30,0,0);
                //Debug.Log(upForce);
            }

            player.transform.LookAt(Lookat.transform);
            DragIndicator.transform.localScale = new Vector3(1, 1, -dragDistance2 / 40);
        }
        else
        {
            DragIndicator.transform.localScale = Vector3.zero;
            upForce = 0;
        }
    }
    void DragDir()
    {

        LocalForceVector = new Vector3(0,-upForce,0);
        dragDistance = Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(CurrDirVector.x, 2) + Mathf.Pow(CurrDirVector.y*1.7f, 2)), 0, 400);
        Playerscript.applyForce(LocalForceVector, dragDistance*forceAmount);
        DragIndicator.transform.localRotation = Quaternion.identity;
    }
    //public void ToggleWinText(bool setted)
    //{
    //    Wintext.gameObject.SetActive(true);
    //}
    public void SetGameState(bool setted)
    {
        GameGoing = setted;
    }
    public void Die()
    {
        playerAlive = false;
    }
    public void Respawn()
    {
        playerAlive = true;
    }
}

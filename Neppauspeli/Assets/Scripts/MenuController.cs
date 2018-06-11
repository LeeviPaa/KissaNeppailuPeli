using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {

    public GameObject dieUI;
    public GameObject WinUI;
    public Text NepCounter;
    public Text TokenCounter;
    public Text Timer;

    public Text FinalFlickCounter;
    public Text FinalTokenCounter;
    public Text FinalTimeCounter;
    public Text FinalTotalPoints;

    public EventSystem es;
    public GameObject finalButton;

    private LevelInstance LI;
    private EventManager EM;
    private bool paused = false;
    private GlobalScoreData_SC levelData;

    private GameObject PauseMenu;

    private void OnEnable()
    {
        if (dieUI != null)
            dieUI.SetActive(false);

        LI = Toolbox.RegisterComponent<LevelInstance>();
        levelData = LI.getLevelScoreData;
        EM = Toolbox.RegisterComponent<EventManager>();
        EM.GameComplete += ListenGameComplete;
        EM.NeppausAmount += ListenNepIncrement;
        EM.TokenAmount += ListenTokenIncrement;
        EM.LevelTimerUpdate += ListenTimerUpdate;
        EM.PlayerDied += PlayerDie;
        EM.PlayerRespawn += PlayerRespawn;
    }
    private void OnDisable()
    {
        EM.GameComplete -= ListenGameComplete;
        EM.NeppausAmount -= ListenNepIncrement;
        EM.TokenAmount -= ListenTokenIncrement;
        EM.LevelTimerUpdate -= ListenTimerUpdate;
        EM.PlayerDied -= PlayerDie;
        EM.PlayerRespawn -= PlayerRespawn;
    }
    public void ListenTimerUpdate(float time)
    {
        if (Timer != null)
            Timer.text = "Time: " + time.ToString();
    }
    private void ListenGameComplete()
    {
        if(es != null && finalButton != null)
            es.SetSelectedGameObject(finalButton);

        WinUI.SetActive(true);
        FinalFlickCounter.text = LI.getFlickPoints().ToString();
        FinalTokenCounter.text = LI.getCollectibleTokenPoints().ToString();
        FinalTimeCounter.text = LI.getTimeMultiplier().ToString();

        int totalPoints = LI.getTotalPoints();
        char[] charArr = totalPoints.ToString().ToCharArray();
        string pointsString = "";

        int c = 0;
        for(int i = charArr.Length-1; i >= 0; i-- )
        {
            c++;
            if(c >= 3)
            {
                pointsString = pointsString.Insert(0, charArr[i].ToString());
                pointsString = pointsString.Insert(0, " ");
                c = 0;
            }
            else
            {
                pointsString = pointsString.Insert(0, charArr[i].ToString());
            }
        }
        FinalTotalPoints.text = pointsString;
    }
    private void ListenNepIncrement(int neps)
    {
        NepCounter.text = "Flicks: "+neps.ToString();
    }
    private void ListenTokenIncrement(int tokens)
    {
        TokenCounter.text = "Gems: "+tokens.ToString();
    }

    void Start () {
        WinUI.SetActive(false);
        bool found = false;
        foreach(Transform child in transform)
        {
            if(child.name == "PauseMenu")
            {
                PauseMenu = child.gameObject;
                child.gameObject.SetActive(false);
                found = true;
            }
        }
        if (!found)
            Debug.LogError(name + " pause menu object not found!");
	}

	void Update () {
		if(Input.GetButtonDown("Cancel"))
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
	}
    public void Pause()
    {
        paused = !paused;
        EM.BroadcastGamePaused();
        PauseMenu.SetActive(true);
    }
    public void Unpause()
    {
        StartCoroutine(UnpauseDelay());
    }
    IEnumerator UnpauseDelay()
    {
        yield return new WaitForSeconds(0.1f);
        paused = !paused;
        EM.BroadcastGameUnPaused();
        PauseMenu.SetActive(false);
        yield return null;
    }
    public void Restart()
    {
        LI.RestartLevel();
        WinUI.SetActive(false);
    }
    public void ReturnToMenu()
    {
        LI.ReturnToMainMenu();
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    bool playerdead = false;
    void PlayerDie()
    {
        playerdead = true;

        StartCoroutine(DUIDelay());
    }
    void PlayerRespawn()
    {
        playerdead = false;
        if (dieUI != null)
            dieUI.SetActive(false);
    }
    IEnumerator DUIDelay()
    {
        yield return new WaitForSeconds(1);
        if (dieUI != null && playerdead)
            dieUI.SetActive(true);
    }
}

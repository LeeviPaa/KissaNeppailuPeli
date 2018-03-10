using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameInstanceData
{
    public float time;
    public int tokens;
    public int flicks;
    public int levelIndex;

}

public class LevelInstance : MonoBehaviour {

    private bool GameGoing = false;
    private EventManager EM;
    private GameInstanceData CurrGameInstance;
    private GameInstanceData PrevGameInstance;
    public GlobalScoreData_SC globalData;
    private delegate void OnLevelWasLoaded();

    private void OnEnable()
    {
        EM = Toolbox.RegisterComponent<EventManager>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        
    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
        StopAllCoroutines();
        if (scene.buildIndex != 0)
        {
            GameGoing = true;

            PrevGameInstance = CurrGameInstance;
            CurrGameInstance = new GameInstanceData();
            CurrGameInstance.time = 0;
            CurrGameInstance.levelIndex = scene.buildIndex;

            StartCoroutine(Timer());

            Debug.LogWarning("levelWasLoaded called");
        }
        else
        {
            GameGoing = false;
        }
    }
    IEnumerator Timer()
    {
        while (GameGoing)
        {
            yield return new WaitForSeconds(1);
            CurrGameInstance.time++;
            EM.BroadcastLevelTimerUpdate((int)CurrGameInstance.time);
            Debug.Log("Timer going!");
        }
    }
#region Public game-control methods
    public void NeppausIncrement()
    {
        CurrGameInstance.flicks++;
        EM.BroadcastNeppausAmount(CurrGameInstance.flicks);
    }
    public void TokenIncrement()
    {
        CurrGameInstance.tokens++;
        EM.BroadcastTokenAmount(CurrGameInstance.tokens);
    }
    public void RestartLevel()
    {
        GameGoing = false;
        PrevGameInstance = CurrGameInstance;
        CurrGameInstance = new GameInstanceData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ReturnToMainMenu()
    {
        GameGoing = false;
        SceneManager.LoadScene(0);
    }
    public void GameComplete()
    {
        GameGoing = false;
        EM.BroadcastGameComplete();
    }
    #endregion
    public int getFlicks
    {
        get
        {
            return CurrGameInstance.flicks;
        }
    }
    public int getTime
    {
        get
        {
            return (int)CurrGameInstance.time;
        }
    }
    public int getTokens
    {
        get
        {
            return CurrGameInstance.tokens;
        }
    }
    public GlobalScoreData_SC getLevelScoreData
    {
        get
        {
            return globalData;
        }
    }
    public int getFlickPoints()
    {
        switch (CurrGameInstance.flicks)
        {
            case 0:
                return 0;
            case 1:
                return globalData.FlickScorePoints[0];
            case 2:
                return globalData.FlickScorePoints[1];
            case 3:
                return globalData.FlickScorePoints[2];
            case 4:
                return globalData.FlickScorePoints[3];
            case 5:
                return globalData.FlickScorePoints[4];
            case 6:
                return globalData.FlickScorePoints[5];
            case 7:
                return globalData.FlickScorePoints[6];
            case 8:
                return globalData.FlickScorePoints[7];
            case 9:
                return globalData.FlickScorePoints[8];
            case 10:
                return globalData.FlickScorePoints[9];
            default:
                return globalData.FlickScorePoints[9];
        }
    }
    public int getCollectibleTokenPoints()
    {
        int points = globalData.collectibleTokenValue * CurrGameInstance.tokens;
        return points;
    }
    public int getTimeMultiplier()
    {
        float multiplier = globalData.inverseTimeMultiplier - ((float)CurrGameInstance.time/10);

        return (int)multiplier;
    }
    public int getTotalPoints()
    {
        int points = (getFlickPoints() + getCollectibleTokenPoints()) * getTimeMultiplier();
        return points;
    }

}

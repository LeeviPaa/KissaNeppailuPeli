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

public struct GameSaveData
{
    public List<LevelSaveData> leveldata;
}
public struct LevelSaveData
{
    public int index;
    public bool locked;
    public int highscore;
}

public class LevelInstance : MonoBehaviour {
    
    private bool GameGoing = false;
    private EventManager EM;
    private GameInstanceData CurrGameInstance;
    private GameInstanceData PrevGameInstance;
    public GlobalScoreData_SC globalData;
    private delegate void OnLevelWasLoaded();

    //persistency
    private GameSaveData saveData;
    public GameSaveData SaveData
    {
        get
        {
            return saveData;
        }
    }

    private void OnEnable()
    {
        //Find savedata
        saveData = LoadGameSaveData();
        Debug.Log("enable");

        EM = Toolbox.RegisterComponent<EventManager>();
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        Debug.Log(SceneManager.sceneCountInBuildSettings.ToString());
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        //save data on disable
        SaveGameData(saveData);
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
        LevelComplete();
    }
    #endregion
    #region gamePoints
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
    #endregion
    #region loading and saving data
    private GameSaveData LoadGameSaveData()
    {
        //if find local save file, load it
        if(false)
        {
            //load and deserialize the data;
        }
        else //else create new save file
        {
            saveData = new GameSaveData();

            int maxScenes = SceneManager.sceneCountInBuildSettings;

            saveData.leveldata = new List<LevelSaveData>();
            for (int i = 1; i < maxScenes; i++)
            {
                LevelSaveData ld = new LevelSaveData();
                ld.index = i;
                if (i == 1)
                {
                    //first level always unlocked
                    ld.locked = false;
                }
                else
                    ld.locked = true;

                ld.highscore = 0;

                //Note: This is the 0th index (1st level)
                saveData.leveldata.Add(ld);
            }
        }

        //whatever happened, we return the savedata object
        return saveData;
        
    }
    private void SaveGameData(GameSaveData data)
    {
        // serialize and save the data
    }
    private void LevelComplete()
    {
        //current scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneIndex);

        //if next scene exists, unlock it
        //next scene index on the list is the same as this scene's index because fuck it
        if ((sceneIndex+1) < SceneManager.sceneCountInBuildSettings)
        {
            LevelSaveData lsd = saveData.leveldata[sceneIndex];
            lsd.locked = false;
            saveData.leveldata[sceneIndex] = lsd;
        }

        //save the data
        SaveGameData(saveData);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Steamworks;

public struct GameInstanceData
{
    public float time;
    public int tokens;
    public int flicks;
    public int levelIndex;

}

[System.Serializable]
public struct GameSaveData
{
    public List<LevelSaveData> leveldata;
}
[System.Serializable]
public struct LevelSaveData
{
    public int index;
    public bool locked;
    public int highscore;
    public bool allGemsCollected;
}

public class LevelInstance : MonoBehaviour {
    
    private bool GameGoing = false;
    private EventManager EM;
    private GameInstanceData CurrGameInstance;
    private GameInstanceData PrevGameInstance;
    public GlobalScoreData_SC globalData;
    private delegate void OnLevelWasLoaded();
    public Animator fadeAtor;
    private LeaderboardManager lbm;
    private CameraShaker CamShake;

    private int levelToLoad = 0;

    //persistant
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
        CamShake = Toolbox.RegisterComponent<CameraShaker>();
        lbm = Toolbox.RegisterComponent<LeaderboardManager>();
        Debug.Log(Application.persistentDataPath);
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
    private bool firstTime = true;
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (firstTime)
        {
            firstTime = false;
        }
        else
        {
            fadeAtor.SetTrigger("FadeIn");
        }

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

            lbm.FindLeaderboardWithSceneIndex(scene.buildIndex);
            
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
            yield return null;
            float t = CurrGameInstance.time + Time.deltaTime;
            CurrGameInstance.time = t;
            EM.BroadcastLevelTimerUpdate(CurrGameInstance.time);
        }
    }
    #region Public game-control methods
    public void NeppausIncrement()
    {
        CamShake.ShakeCamera(0.24f, 0.1f, 1, 10);
        CurrGameInstance.flicks++;
        EM.BroadcastNeppausAmount(CurrGameInstance.flicks);
    }
    public void TokenIncrement()
    {
        CamShake.ShakeCamera(1, 1, 0.5f);
        CurrGameInstance.tokens++;
        EM.BroadcastTokenAmount(CurrGameInstance.tokens);
    }
    public void RestartLevel()
    {
        GameGoing = false;
        PrevGameInstance = CurrGameInstance;
        CurrGameInstance = new GameInstanceData();

        LoadLevelIndex(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReturnToMainMenu()
    {
        GameGoing = false;

        LoadLevelIndex(0);
    }
    public void GameComplete()
    {
        GameGoing = false;
        EM.BroadcastGameComplete();
        LevelComplete();
        lbm.UploadScore(getTotalPoints());
    }
    public void LoadLevelIndex(int index)
    {
        if (index >= SceneManager.sceneCountInBuildSettings)
            Debug.LogError("SceneWith scene index: " + index + " not found!");
        else
        {
            levelToLoad = index;
            fadeAtor.SetTrigger("FadeOut");
            EM.BroadcastLevelFadeStart();
        }

    }
    public void FadeOutComplete()
    {
        SceneManager.LoadScene(levelToLoad);
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
    public float getTime
    {
        get
        {
            return CurrGameInstance.time;
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
    public float getTimeMultiplier()
    {
        float multiplier = globalData.inverseTimeMultiplier - (CurrGameInstance.time/10);

        return multiplier;
    }
    public int getTotalPoints()
    {
        int points = (int)((getFlickPoints() + getCollectibleTokenPoints()) * getTimeMultiplier());
        if(points >= 6000000)
        {
            EM.BroadcastAchivementMillionare();
        }
        return points;
    }
    #endregion
    #region loading and saving data
    private GameSaveData LoadGameSaveData()
    {
        bool deserializeSuccessfull = true;

        GameSaveData newSaveData = new GameSaveData();


        //if find local save file, load it
        if (File.Exists(Application.persistentDataPath + "/playerdata.dat"))
        {
            //load and deserialize the data;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerdata.dat", FileMode.Open);
            try
            {
                newSaveData = (GameSaveData)bf.Deserialize(file);
            }
            catch
            {
                deserializeSuccessfull = false;
                Debug.LogError("Unable to load save data");
            }

            file.Close();
        }
        else
        {
            deserializeSuccessfull = false;
        }
            
        
        if(!deserializeSuccessfull) //if not successfull, create new save file
        {

            int maxScenes = SceneManager.sceneCountInBuildSettings;

            newSaveData.leveldata = new List<LevelSaveData>();
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
                ld.allGemsCollected = false;

                //Note: This is the 0th index (1st level)
                newSaveData.leveldata.Add(ld);
            }

            SaveGameData(newSaveData);
        }

        //whatever happened, we return the savedata object
        return newSaveData;
        
    }
    private void SaveGameData(GameSaveData data)
    {
        
        // serialize and save the data
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if(File.Exists(Application.persistentDataPath + "/playerdata.dat"))
            file = File.Open(Application.persistentDataPath + "/playerdata.dat", FileMode.Open);
        else
            file = File.Create(Application.persistentDataPath + "/playerdata.dat");
        
        bf.Serialize(file, data);
        file.Close();
    }
    private void LevelComplete()
    {
        //current scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneIndex);

        if(sceneIndex == 14)
        {
            EM.BroadcastAchivementFinalLevel();
        }
        //if next scene exists, unlock it
        //next scene index on the list is the same as this scene's index because fuck it
        if ((sceneIndex+1) < SceneManager.sceneCountInBuildSettings)
        {
            LevelSaveData lsd = saveData.leveldata[sceneIndex];
            lsd.locked = false;
            saveData.leveldata[sceneIndex] = lsd;
        }
        if(sceneIndex < SaveData.leveldata.Count)
        {
            LevelSaveData lsd = saveData.leveldata[sceneIndex];
            if(lsd.highscore < getTotalPoints())
            {
                lsd.highscore = getTotalPoints();
            }
        }

        //save the data
        SaveGameData(saveData);
    }
    public void maxGemsReached()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex <= SaveData.leveldata.Count)
        {
            LevelSaveData lsd = saveData.leveldata[sceneIndex-1];
            lsd.allGemsCollected = true;
            saveData.leveldata[sceneIndex-1] = lsd;
            EM.BroadcastMaxGemsReachedOnMap(sceneIndex);
            SaveGameData(SaveData);
            print("max gems reached!");
        }
    }
    #endregion


    public void ExitApplication()
    {
        Application.Quit();
    }
}

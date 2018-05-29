using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

[System.Serializable]
public struct SceneListProperties
{
    public Transform cameraTarget;
    public int SceneIndex;
    public bool unlocked;
}

public class MainMenuController : MonoBehaviour {

    public GameObject winText;
    public GameObject nepCounter;
    public GameObject MainCamera;
    private int currIndex = 0;

    public float lerpSpeed = 10;

    public List<SceneListProperties> Scenelist = new List<SceneListProperties>();

    private LevelInstance LI;
    private EventManager EM;

    //steamAPI variables
    private SteamLeaderboard_t m_CurrentLeaderboard;

    public int m_nLeaderboardEntries;
    public LeaderboardEntry_t[] m_leaderboardentries = new LeaderboardEntry_t[10];

    protected Callback<LeaderboardFindResult_t> m_callResultFindLeaderboard;
    protected Callback<LeaderboardScoreUploaded_t> m_callResulUploadScore;
    protected Callback<LeaderboardScoresDownloaded_t> m_callResultDownloadScore;


    #region MonoBehaviour
    private void OnEnable()
    {
        LI = Toolbox.RegisterComponent<LevelInstance>();
        EM = Toolbox.RegisterComponent<EventManager>();

        SceneManager.sceneLoaded += FormatLevelList;

        if (!SteamManager.Initialized)
            return;

        m_callResultFindLeaderboard = Callback<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
        m_callResulUploadScore = Callback<LeaderboardScoreUploaded_t>.Create(OnUploadSCore);
        m_callResultDownloadScore = Callback<LeaderboardScoresDownloaded_t>.Create(OnDownloadScore);
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FormatLevelList;
    }

    void Start () {
        if (MainCamera == null)
            Debug.LogError("MainCamera not found!!");

        
	}
    #endregion

    #region internal methods
    void FormatLevelList(Scene s, LoadSceneMode m)
    {
        //coroutine is because the object is being created on the same frame or something
        StartCoroutine(delaySceneCheck());
    }
    private IEnumerator delaySceneCheck()
    {
        yield return null;
        // here we check which levels are locked and which are not

        //Level 1 always is unlocked;
        UnlockLevel(1);

        foreach (LevelSaveData lsd in LI.SaveData.leveldata)
        {
            if (!lsd.locked)
            {
                UnlockLevel(lsd.index);
            }
        }
    }
	
	void Update () {
        if (Scenelist[currIndex].cameraTarget != null)
        {
            MainCamera.transform.position = Vector3.Lerp(
                MainCamera.transform.position,
                Scenelist[currIndex].cameraTarget.position,
                lerpSpeed * Time.deltaTime);
        }
        else
            Debug.LogError("CameraTarget not found!!");


        if (Input.GetKeyDown(KeyCode.Keypad2))
            UnlockLevel(2);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            UnlockLevel(3);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            UnlockLevel(4);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            UnlockLevel(5);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            UnlockLevel(6);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            UnlockLevel(7);
        if (Input.GetKeyDown(KeyCode.Keypad8))
            UnlockLevel(8);
        if (Input.GetKeyDown(KeyCode.Keypad9))
            UnlockLevel(9);
        if (Input.GetKeyDown(KeyCode.Keypad0))
            UnlockLevel(10);
        if (Input.GetKeyDown(KeyCode.Keypad1) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(11);
        if (Input.GetKeyDown(KeyCode.Keypad2) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(12);
        if (Input.GetKeyDown(KeyCode.Keypad3) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(13);
        if (Input.GetKeyDown(KeyCode.Keypad4) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(14);
        if (Input.GetKeyDown(KeyCode.Keypad5) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(15);
        if (Input.GetKeyDown(KeyCode.Keypad6) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(16);
        if (Input.GetKeyDown(KeyCode.Keypad7) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(17);
        if (Input.GetKeyDown(KeyCode.Keypad8) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(18);
        if (Input.GetKeyDown(KeyCode.Keypad9) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(19);
        if (Input.GetKeyDown(KeyCode.Keypad0) && Input.GetKey(KeyCode.RightControl))
            UnlockLevel(20);
    }

    public void TryMapRight()
    {
        if(currIndex+1 < Scenelist.Count)
        {
            currIndex++;
        }
    }
    public void TryMapLeft()
    {
        if(currIndex > 0)
        {
            currIndex--;
        }
    }
    public void PlayMap()
    {
        if (Scenelist[currIndex].unlocked)
        {
            LI.LoadLevelIndex(Scenelist[currIndex].SceneIndex);
        }
    }
    public void UnlockLevel(int indx)
    {
        int i = indx - 1;
        if (i < Scenelist.Count)
        {
            SceneListProperties pr = Scenelist[i];
            pr.unlocked = true;
            Scenelist[i] = pr;

            Locked lc = pr.cameraTarget.parent.GetComponentInChildren<Locked>();
            if (lc != null)
            {
                lc.Unlock();
            }
        }
    }
    #endregion

    #region steamAPI 
    /// <summary>
    /// find leaderboards
    /// </summary>
    /// <param name="pchLearderboardName"></param>
    void FindLeaderboard( string pchLearderboardName)
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(pchLearderboardName);
        
    }
    void OnFindLeaderboard(LeaderboardFindResult_t pCallback)
    {
        if(pCallback.m_bLeaderboardFound == 0)
        {
            print("Leaderboard could not be found");
            return;
        }

        m_CurrentLeaderboard = pCallback.m_hSteamLeaderboard;
    }

    /// <summary>
    /// upload score
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    bool UploadScore(int score)
    {
        if (m_CurrentLeaderboard == null)
            return false;

        SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(
            m_CurrentLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);

        return true;
    }
    void OnUploadSCore(LeaderboardScoreUploaded_t pCallback)
    {
        if(pCallback.m_bSuccess == 0)
        {
            print("Score could not be uploaded to steam");
        }
    }

    /// <summary>
    /// download scores
    /// </summary>
    /// <returns></returns>
    bool DownloadScores()
    {
        if(m_CurrentLeaderboard == null)
        {
            return false;
        }

        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(
            m_CurrentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -4, 5);

        return true;

    }
    void OnDownloadScore(LeaderboardScoresDownloaded_t pCallback)
    {

        m_nLeaderboardEntries = Mathf.Min(pCallback.m_cEntryCount, 10);

        for (int i = 0; i < m_nLeaderboardEntries; i++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(
                pCallback.m_hSteamLeaderboardEntries, i, out m_leaderboardentries[i], null, 0);
        }
        
    }
    #endregion
}

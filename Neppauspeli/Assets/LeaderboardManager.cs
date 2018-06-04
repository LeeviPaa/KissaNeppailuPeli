using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class LeaderboardManager : MonoBehaviour
{
    //steamAPI variables
    private SteamLeaderboard_t m_CurrentLeaderboard;

    public int m_nLeaderboardEntries;
    public LeaderboardEntry_t[] m_leaderboardentries = new LeaderboardEntry_t[10];

    protected CallResult<LeaderboardFindResult_t> m_callResultFindLeaderboard = new CallResult<LeaderboardFindResult_t>();
    protected CallResult<LeaderboardScoreUploaded_t> m_callResulUploadScore = new CallResult<LeaderboardScoreUploaded_t>();
    protected CallResult<LeaderboardScoresDownloaded_t> m_callResultDownloadScore = new CallResult<LeaderboardScoresDownloaded_t>();

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        //m_callResultFindLeaderboard = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
        //m_callResulUploadScore = CallResult<LeaderboardScoreUploaded_t>.Create(OnUploadSCore);
        //m_callResultDownloadScore = CallResult<LeaderboardScoresDownloaded_t>.Create(OnDownloadScore);
    }

   
    public void FindLeaderboardWithSceneIndex(int index)
    {
        switch (index)
        {
            case 1:
                FindLeaderboard("L1_Highscore");
                break;
            case 2:
                FindLeaderboard("L2_Highscore");
                break;
            case 3:
                FindLeaderboard("L3_Highscore");
                break;
            case 4:
                FindLeaderboard("L4_Highscore");
                break;
            case 5:
                FindLeaderboard("L5_Highscore");
                break;
            case 6:
                FindLeaderboard("L6_Highscore");
                break;
            case 7:
                FindLeaderboard("L7_Highscore");
                break;
            case 8:
                FindLeaderboard("L8_Highscore");
                break;
            case 9:
                FindLeaderboard("L9_Highscore");
                break;
            case 10:
                FindLeaderboard("L10_Highscore");
                break;
            case 11:
                FindLeaderboard("L11_Highscore");
                break;
            case 12:
                FindLeaderboard("L12_Highscore");
                break;
            case 13:
                FindLeaderboard("L13_Highscore");
                break;
            case 14:
                FindLeaderboard("L14_Highscore");
                break;
            case 15:
                FindLeaderboard("L15_Highscore");
                break;
            default:
                break;
        }
    }
    #region steamAPI 
    /// <summary>
    /// find leaderboards
    /// </summary>
    /// <param name="pchLearderboardName"></param>
    void FindLeaderboard(string pchLearderboardName)
    {
        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(pchLearderboardName);
        m_callResultFindLeaderboard.Set(hSteamAPICall, OnFindLeaderboard);
    }
    void OnFindLeaderboard(LeaderboardFindResult_t pCallback, bool failure)
    {
        if (pCallback.m_bLeaderboardFound == 0)
        {
            print("Leaderboard could not be found");
            return;
        }
        else
            print("leaderboard found!");

        m_CurrentLeaderboard = pCallback.m_hSteamLeaderboard;
    }

    /// <summary>
    /// upload score
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public bool UploadScore(int score)
    {
        if (m_CurrentLeaderboard == null)
            return false;

        SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(
            m_CurrentLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
        m_callResulUploadScore.Set(hSteamAPICall, OnUploadSCore);
        return true;
    }
    void OnUploadSCore(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        if (pCallback.m_bSuccess == 0)
        {
            print("Score could not be uploaded to steam");
        }
        else
            print("Upload score success!");
    }

    /// <summary>
    /// download scores
    /// </summary>
    /// <returns></returns>
    bool DownloadScores()
    {
        if (m_CurrentLeaderboard == null)
        {
            return false;
        }

        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(
            m_CurrentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -4, 5);
        m_callResultDownloadScore.Set(hSteamAPICall, OnDownloadScore);
        return true;

    }
    void OnDownloadScore(LeaderboardScoresDownloaded_t pCallback, bool success)
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

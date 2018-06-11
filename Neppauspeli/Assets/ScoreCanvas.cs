using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;

public class ScoreCanvas : MonoBehaviour {

    public Text upName;
    public Text upScore;
    public Text upPosition;

    public Text myName;
    public Text myScore;
    public Text myPosition;

    public Text downName;
    public Text downScore;
    public Text downPosition;

    public Text scoreName;

    public int thisMap = 1;
    
    //steamAPI variables
    private SteamLeaderboard_t m_CurrentLeaderboard;

    private int m_nLeaderboardEntries;
    public LeaderboardEntry_t[] m_leaderboardentries = new LeaderboardEntry_t[10];

    protected CallResult<LeaderboardFindResult_t> m_callResultFindLeaderboard = new CallResult<LeaderboardFindResult_t>();
    protected CallResult<LeaderboardScoresDownloaded_t> m_callResultDownloadScore = new CallResult<LeaderboardScoresDownloaded_t>();

    private void Start()
    {
        if (!SteamManager.Initialized)
            return;

        scoreName.text = "Level " + thisMap;

        switch (thisMap)
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

        DownloadScores();
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

            if(m_leaderboardentries[i].m_steamIDUser == SteamUser.GetSteamID())
            {
                setMyTexts(m_leaderboardentries[i]);
                continue;
            }

            switch(i)
            {
                case 0:
                    setDownTexts(m_leaderboardentries[i]);
                    break;
                case 7:
                    setUpTexts(m_leaderboardentries[i]);
                    break;
                default:
                    break;
            }
        }
    }
    void setMyTexts(LeaderboardEntry_t entry)
    {
        myName.text = SteamFriends.GetPersonaName();
        myPosition.text = entry.m_nGlobalRank.ToString();

        int totalPoints = entry.m_nScore;
        char[] charArr = totalPoints.ToString().ToCharArray();
        string pointsString = "";

        int c = 0;
        for (int i = charArr.Length - 1; i >= 0; i--)
        {
            c++;
            if (c >= 3)
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
        myScore.text = pointsString;
    }
    void setUpTexts(LeaderboardEntry_t entry)
    {
        upName.text = "Above";
        upPosition.text = entry.m_nGlobalRank.ToString();

        int totalPoints = entry.m_nScore;
        char[] charArr = totalPoints.ToString().ToCharArray();
        string pointsString = "";

        int c = 0;
        for (int i = charArr.Length - 1; i >= 0; i--)
        {
            c++;
            if (c >= 3)
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
        upScore.text = pointsString;
    }
    void setDownTexts(LeaderboardEntry_t entry)
    {
        downName.text = "Below";
        downPosition.text = entry.m_nGlobalRank.ToString();

        int totalPoints = entry.m_nScore;
        char[] charArr = totalPoints.ToString().ToCharArray();
        string pointsString = "";

        int c = 0;
        for (int i = charArr.Length - 1; i >= 0; i--)
        {
            c++;
            if (c >= 3)
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
        downScore.text = pointsString;
    }
}

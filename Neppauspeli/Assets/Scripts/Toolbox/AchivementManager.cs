using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

class AchivementManager : MonoBehaviour
{ 

    //achivement ID
    private enum Achievement : int
    {
        ACH_MILLIONPOINTS = 2,
        ACH_FINALLEVEL = 3,
    };

    private Dictionary<Achievement, Achievement_t> m_Achievements = new Dictionary<Achievement, Achievement_t>();

    private CGameID m_GameID;

    // Did we get the stats from Steam?
    private bool m_bRequestedStats;
    private bool m_bStatsValid;

    // Should we store stats this frame?
    private bool m_bStoreStats;

    // Persisted Stat details
    private int gamesPlayed;
    private int gemsCollected;


    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    EventManager em;
    SteamManager sm;
    protected void Awake()
    {
        Achievement_t n = new Achievement_t(Achievement.ACH_MILLIONPOINTS, "Millionare", "");
        m_Achievements.Add(Achievement.ACH_MILLIONPOINTS, n);

        n = new Achievement_t(Achievement.ACH_FINALLEVEL, "Completionist", "");
        m_Achievements.Add(Achievement.ACH_FINALLEVEL, n);

    }
    protected void OnEnable()
    {
        if (!SteamManager.Initialized)
            return;

        // Cache the GameID for use in the Callbacks
        m_GameID = new CGameID(SteamUtils.GetAppID());

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

        // These need to be reset to get the stats upon an Assembly reload in the Editor.
        m_bRequestedStats = false;
        m_bStatsValid = false;

        UpdateStats();

        em = Toolbox.RegisterComponent<EventManager>();
        
        em.GameComplete += Stat_GamesPlayed;
        em.TokenAmount += Stat_GemsCollected;
        em.Achivement_FinalLevel += Achivement_FinalLevel;
        em.Achivement_Millionare += Achivement_Millionare;
    }
    protected void OnDisable()
    {
        em.GameComplete -= Stat_GamesPlayed;
        em.TokenAmount -= Stat_GemsCollected;
        em.Achivement_FinalLevel -= Achivement_FinalLevel;
        em.Achivement_Millionare -= Achivement_Millionare;
    }

    private void UpdateStats()
    {
        if (!m_bRequestedStats)
        {
            // Is Steam Loaded? if no, can't get stats, done
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }

            // If yes, request our stats
            bool bSuccess = SteamUserStats.RequestCurrentStats();

            // This function should only return false if we weren't logged in, and we already checked that.
            // But handle it being false again anyway, just ask again later.
            m_bRequestedStats = bSuccess;
        }
    }

    #region Achivement methods
    protected  void Achivement_Millionare()
    {

        UnlockAchievement(m_Achievements[Achievement.ACH_MILLIONPOINTS]);
    }
    protected void Achivement_FinalLevel()
    {
        UnlockAchievement(m_Achievements[Achievement.ACH_FINALLEVEL]);
    }
    #endregion
    #region Stat methods 
    protected void Stat_GamesPlayed()
    {
        //A stat of the amount of games played in general
        //Name: Games Played
        gamesPlayed++;
        SetStat("Games", gamesPlayed);
    }
    protected void Stat_GemsCollected(int gems)
    {
        //A stat of the amount of games played in general
        //Name: Gems Collected
        gemsCollected++;
        SetStat("Gems", gemsCollected);
    }
    
    #endregion


    #region steamAPI methods and callbacks
    //-----------------------------------------------------------------------------
    // Purpose: We have stats data from Steam. It is authoritative, so update
    //			our data with those results now.
    //-----------------------------------------------------------------------------
    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized)
            return;

        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                m_bStatsValid = true;

                // load achievements
                foreach (KeyValuePair<Achievement, Achievement_t> ach in m_Achievements)
                {
                    bool ret = SteamUserStats.GetAchievement(ach.Value.m_eAchievementID.ToString(), out ach.Value.m_bAchieved);
                    if (ret)
                    {
                        ach.Value.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.Value.m_eAchievementID.ToString(), "name");
                        ach.Value.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.Value.m_eAchievementID.ToString(), "desc");
                    }
                    else
                    {
                        Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.Value.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
                    }
                }

                // load stats
                SteamUserStats.GetStat("Games", out gamesPlayed);
                SteamUserStats.GetStat("Gems", out gemsCollected);
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Our stats data was stored!
    //-----------------------------------------------------------------------------
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                // One or more stats we set broke a constraint. They've been reverted,
                // and we should re-iterate the values now to keep in sync.
                Debug.Log("StoreStats - some failed to validate");
                // Fake up a callback here so that we re-load the values.
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Unlock this achievement
    //-----------------------------------------------------------------------------
    private void UnlockAchievement(Achievement_t achievement)
    {
        if (!SteamManager.Initialized)
            return;

        achievement.m_bAchieved = true;

        // the icon may change once it's unlocked
        //achievement.m_iIconImage = 0;

        // mark it down
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

        // Store stats end of frame
        m_bStoreStats = true;

        Debug.Log(achievement.m_eAchievementID.ToString());
    }

    /// <summary>
    /// Purpose: store a stat
    /// </summary>
    private void SetStat(string name, int value)
    {
        if (!SteamManager.Initialized)
            return;
        Debug.Log("stat changed: " + name + " " + value);
        SteamUserStats.SetStat(name, value);

        bool bSuccess = SteamUserStats.StoreStats();
        if (!bSuccess)
        {
            Debug.LogError("Storing stats unsuccessfull!");
        }

    }

    //-----------------------------------------------------------------------------
    // Purpose: An achievement was stored
    //-----------------------------------------------------------------------------
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        // We may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }
    #endregion
    private class Achievement_t
    {
        public Achievement m_eAchievementID;
        public string m_strName;
        public string m_strDescription;
        public bool m_bAchieved;

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        public Achievement_t(Achievement achievementID, string name, string desc)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_bAchieved = false;
        }
    }
}
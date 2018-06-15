using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ExampleEventDelegate();
    public delegate void Void();
    public delegate void IntVoid(int i);
    public delegate void FloatVoid(float f);

    public event ExampleEventDelegate exampleEvent;
    public event Void gamePaused;
    public event Void gameUnPaused;

    public event IntVoid TokenAmount;
    public event IntVoid NeppausAmount;
    public event FloatVoid LevelTimerUpdate;

    public event Void GameComplete;

    public void CallExampleEvent()
    {
        exampleEvent();
    }
    public void BroadcastGamePaused()
    {
        if(gamePaused != null)
        {
            gamePaused();
        }
    }
    public void BroadcastGameUnPaused()
    {
        if(gameUnPaused != null)
        {
            gameUnPaused();
        }
    }
    /// <summary>
    /// Broadcasts the amount of tokens collected. This is broadcasted by the levelInstance each time a token is collected.
    /// </summary>
    /// <param name="amount"></param>
    public void BroadcastTokenAmount(int amount)
    {
        if(TokenAmount != null)
        {
            TokenAmount(amount);
        }
    }
    /// <summary>
    /// Broadcasts the amount of flicks played. This is broadcasted by the levelInstance each time a player flicks the character.
    /// </summary>
    /// <param name="amount"></param>
    public void BroadcastNeppausAmount(int neps)
    {
        if(NeppausAmount != null)
        {
            NeppausAmount(neps);
        }
    }
    public void BroadcastGameComplete()
    {
        if(GameComplete != null)
        {
            GameComplete();
        }
    }
    public void BroadcastLevelTimerUpdate(float currTime)
    {
        if(LevelTimerUpdate != null)
        {
            LevelTimerUpdate(currTime);
        }
    }

    public event Void PlayerDied;
    public void BroadcastPlayerDied()
    {
        if(PlayerDied != null)
        {
            PlayerDied();
        }
    }
    public event Void PlayerRespawn;
    public void BroadcastPlayerRespawn()
    {
        if(PlayerRespawn != null)
        {
            PlayerRespawn();
        }
    }

    public event Void Achivement_Millionare;
    public void BroadcastAchivementMillionare()
    {
        if(Achivement_Millionare != null)
        {
            Achivement_Millionare();
        }
    }
    public event Void Achivement_FinalLevel;
    public void BroadcastAchivementFinalLevel()
    {
        if(Achivement_FinalLevel!= null)
        {
            Achivement_FinalLevel();
        }
    }

    public event Void LevelFadeStart;
    public void BroadcastLevelFadeStart()
    {
        if(LevelFadeStart != null)
        {
            LevelFadeStart();
        }
    }

    public event IntVoid MaxGemsReachedOnMap;
    public void BroadcastMaxGemsReachedOnMap(int index)
    {
        if(MaxGemsReachedOnMap != null)
        {
            MaxGemsReachedOnMap(index);
        }
    }

}

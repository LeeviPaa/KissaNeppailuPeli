using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LSCD", menuName = "Level/LevelData", order = 3)]
public class GlobalScoreData_SC : ScriptableObject
{
    public int collectibleTokenValue = 100000;
    /// <summary>
    /// Time multiplier is counted by dividing this value by the time in seconds.
    /// </summary>
    public int inverseTimeMultiplier = 100;

    public List<int> FlickScorePoints = new List<int>(10);
}

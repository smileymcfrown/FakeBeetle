using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public static LevelData openLevel;
    public string[,] layout;
    public string levelName;
    public int turns;
    public string background;

    public LevelData ()
    {
        layout = new string[13, 9];
        levelName = "";
        turns = 0;
        background = "temp_level_opaque";
    }
}
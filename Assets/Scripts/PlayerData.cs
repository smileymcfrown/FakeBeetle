using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public static PlayerData player;

    public int currentLevel;
    public int score;

    public PlayerData ()
    {
        currentLevel = 0;
        score = 0;
    }
}

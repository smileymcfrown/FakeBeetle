using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        // Load all the levels from file at start - could have been done from Levels.cs - done here
        // as a "Custom Game" menu item was to be made in which the player could re-arrange
        // the level to play their own custom levels in any order.

        LoadSave.Load(); 
        //PlayerData.player = new PlayerData();

    }

    public void StartGame() // Loads The Level selector once the "Start" Button is clicked.
    {
        SceneManager.LoadScene("Level");
    }

    public void LevelEditor() // Loads level one when "LEVEL 1" is clicked
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
        



}

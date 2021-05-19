using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    void Start()
    {
        /* Load all the levels from file at start - could have been done from Levels.cs - done here
         * as a "Custom Game" menu item was to be made in which the player could re-arrange
         * the level to play their own custom levels in any order.
         */

        LoadSave.Load();

        // Couldn't get PlayerData class to carry score and current level to work
        // PlayerData.player = new PlayerData();



        /* Trying to get Full Screen toggle to get checkmark to show up correctly...
         * Windows remembers a program's last window size in the registry, so having the toggle.isOn set to 'false'
         * in the inspector won't work.. the below code also does not work because it messes up the checkmark when
         * returning to the main menu from a game. Also tried in FullScreen() below.
         *
         * Ended up removing checkmark for now... how to fix this? 
         */

         // if (Screen.fullScreen == true) { GameObject.Find("FullScreen").GetComponent<Toggle>().isOn = true; }
         // else { GameObject.Find("FullScreen").GetComponent<Toggle>().isOn = false; }
         


    }

    public void StartGame() // Loads The Level selector once the "Start" Button is clicked.
    {
        SceneManager.LoadScene("Level");
    }

    public void LevelEditor() // Loads level one when "LEVEL 1" is clicked
    {
        SceneManager.LoadScene("LevelEditor");
    }

    public void FullScreen()
    {
        //Toggle screenToggle = GameObject.Find("FullScreen").GetComponent<Toggle>();
        //if (screenToggle.isOn) { Screen.fullScreen = false; screenToggle.isOn = false; }
        //else { Screen.fullScreen = true; screenToggle.isOn = true; }

        Screen.fullScreen = !Screen.fullScreen;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
        
    
}

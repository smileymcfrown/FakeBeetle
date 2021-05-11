using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
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

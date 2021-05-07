using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame() // Loads The Level selector once the "Start" Button is clicked.
    {
        SceneManager.LoadScene("StartButtonScene");
    }

    public void LevelOne() // Loads level one when "LEVEL 1" is clicked
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
        



}

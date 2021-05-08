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

    public void Level1() // Loads level one when "LEVEL 1" is clicked
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void Options()
    {
        SceneManager.LoadScene("GraphicsSettings");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2");
    }    

    public void Level3()
    {
        SceneManager.LoadScene("Level3");
    }
        



}

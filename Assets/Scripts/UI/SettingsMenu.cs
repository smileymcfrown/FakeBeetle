using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TurnCounter turnsCounter;
    public AudioMixer audioMixer;
    [SerializeField]
    private bool isPaused = false;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject deathPanel;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if(turnsCounter.turns<=0)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
            //Debug.Log("dead");
        }
    }
    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Quit()
    {
        Application.Quit();
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    GameObject child;
    public Text timeText;
    public Text scoreText;

    private void OnEnable()
    {
        StartCoroutine(SlowMenu());
    }

    public void Leave(bool restart)
    {
        Debug.Log("Inside StartAgain");

        Level.currentLevel = 0;
        Level.score = 0;
        LevelData.openLevel = LoadSave.savedLevels[0];
        PlayerMovement.tempLayout = LevelData.openLevel.layout;
        if (restart) { SceneManager.LoadScene("Level"); }
        else { SceneManager.LoadScene("MainMenu"); }
    }

    /* public void StartAgain()
     {
         Debug.Log("Inside StartAgain");

         Level.currentLevel = 0;
         Level.score = 0;
         //LevelData.openLevel = LoadSave.savedLevels[0];
         SceneManager.LoadScene("Level");
     }
     public void MainMenu()
     {
         Debug.Log("Inside Quit");

         Level.currentLevel = 0;
         Level.score = 0;
         LevelData.openLevel = LoadSave.savedLevels[0];
         SceneManager.LoadScene("MainMenu");
     }
    */

    IEnumerator SlowMenu()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            int result = 0;
            float wait;
            child = transform.GetChild(i).gameObject;
            child.SetActive(true);
            Debug.Log("Inside EndSlowMenu");

            if (child.name == "Epilogue")
            {
                Debug.Log("Inside Epilogue");
                wait = 3f;
            }
            else if (child.name == "SubEpilogue")
            {
                Debug.Log("Inside SubEpi");
                wait = 4f;
            }
            else if (child.name == "TimeResult")
            {
                if (Level.timeTaken >= 1500)
                {
                    result = 0; //PlayerData.player
                }
                else
                {
                    result = 15000 - ((int)Level.totalTime * 10); //PlayerData.player.
                    Level.score += result;
                }
                wait = 3.5f;
                StartCoroutine(IncrementPoints(result, wait, timeText));
            }
            else if (child.gameObject.name == "ScoreResult")
            {
                wait = 3.5f;
                StartCoroutine(IncrementPoints(  //PlayerData.player
                Level.score, wait, scoreText));
            }
            else if (child.gameObject.name == "StartAgain")
            {
                wait = 1f;
                EventSystem.current.SetSelectedGameObject(child.gameObject);
            }
            else { wait = 1f; }
            Debug.Log("Just before yield");

            yield return new WaitForSeconds(wait);
        }
    }

    private IEnumerator IncrementPoints(int result, float wait, Text resultText)
    {
        int startScore = 0;
        float time = 0;
        float countTime;

        //if (result < 100) { countTime = 1.5; }
        //else { countTime = 3; }
        countTime = wait - 0.5f;

        resultText.text = startScore.ToString();

        while (time < countTime)
        {
            yield return null;
            time += Time.deltaTime;
            float factor = time / countTime;
            resultText.text = ((int)Mathf.Lerp(startScore, result, factor)).ToString();
        }
        yield break;
    }
}
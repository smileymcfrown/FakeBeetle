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

    // Script runs when Game Complete UI Panel is activated and loads each item one at a time.
    private void OnEnable()
    {
        StartCoroutine(SlowMenu());
    }

    // Either return to main menu or reset variables and start game again
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

    // Coroutine to run through menu items loading them with a delay depending on the item.
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

    // Coroutine to make points counts up from zero
    private IEnumerator IncrementPoints(int result, float wait, Text resultText)
    {
        int startScore = 0;
        float time = 0;
        float countTime;

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
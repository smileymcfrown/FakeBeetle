using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    GameObject child;
    public Text turnsText;
    public Text bonusText;
    public Text timeText;
    public Text totalText;
    public GameObject completedPanel;

    // Script runs when Game Complete UI Panel is activated and runs coroutine to load each item one at a time.
    private void OnEnable()
    {
       StartCoroutine(SlowMenu());
    }

    // Does what it says
    public void NextLevel()
    {
        //Check if level is the final level and either activate Game Complete panel or go to next level
        if(LevelData.openLevel.levelName == "Egypt")
        {
            //level.GameComplete(); - Couldn't get this to work

            completedPanel.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else { Level.currentLevel++;  SceneManager.LoadScene("Level"); }
    }

    // Goes back to the main menu clearing game variables
    public void QuitGame()
    {
        //PlayerData.player.currentLevel = 0;  - doesn't work

        Level.currentLevel = 0;
        Level.score = 0;
        SceneManager.LoadScene("MainMenu");
    }

    // Coroutine to run through menu items loading them with a delay depending on the item.
    IEnumerator SlowMenu()
    {
        Debug.Log("Inside SlowMenu");
        
        // Cycle through child elements and set delays after each item
        for (int i = 0; i < transform.childCount; ++i)
        {
            int result = 0;
            float wait;
            child = transform.GetChild(i).gameObject;


            child.SetActive(true);
            
            if (child.name == "TurnsResult") 
            { 
                result = Level.turnsRemaining;
                wait = 2.5f;
                StartCoroutine(IncrementPoints(result, wait, turnsText));
                
                
            }
            else if (child.name == "BonusResult") 
            { 
                result = 350 * Level.turnsRemaining;
                //PlayerData.player.
                    Level.score += result;
                wait = 3.5f;
                StartCoroutine(IncrementPoints(result,wait, bonusText));
                            }
            else if (child.name == "TimeResult")
            {
                if (Level.timeTaken >= 600)
                { result = 0; //PlayerData.player
                        Level.score += result; }
                else 
                { result = 6000 - ((int)Level.timeTaken * 10); //PlayerData.player.
                        Level.score += result; }
                wait = 3.5f;
                StartCoroutine(IncrementPoints(result, wait, timeText));
            }
            else if (child.gameObject.name == "TotalResult")
            {
                wait = 3.5f;
                StartCoroutine(IncrementPoints(//PlayerData.player
                Level.score, wait, totalText));
            }
            else if (child.gameObject.name == "NextLevel")
            {
                if (LevelData.openLevel.levelName == "Egypt")
                {
                    child.GetComponentInChildren<Text>().text = "Arrive home";
                }
                wait = 1f;
                EventSystem.current.SetSelectedGameObject(child.gameObject);
            }
            else { wait = 1f; }

            yield return new WaitForSeconds(wait);
        }
    }

    // SubCoroutine to show points increasing from 0 to the result
    private IEnumerator IncrementPoints(int result, float wait, Text resultText)
    {
        float countTime;
        countTime = wait - 0.5f;
        resultText.text = "0";
        float time = 0;

        // Run a linear interpolation to show all numbers from 0 to 'result' in the specified time.
        while (time < countTime)
        {
            yield return null;
            time += Time.deltaTime;
            float factor = time / countTime;
            resultText.text = ((int)Mathf.Lerp(0, result, factor)).ToString();
        }

        yield break;
    }
}

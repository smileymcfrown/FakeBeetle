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
    //public Button nextButton;

    void Start()
    {
    }
  
    private void OnEnable()
    {
        Debug.Log("Inside OnEnable");

        StartCoroutine(SlowMenu());
    }

    public void NextLevel()
    {
        Debug.Log("Inside NextLevel");
        //PlayerData.player
        if(LevelData.openLevel.levelName == "Egypt")
        {
            completedPanel.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
            //level.GameComplete();
        }
        else { Level.currentLevel++;  SceneManager.LoadScene("Level"); }
    }
    public void QuitGame()
    {
        Debug.Log("Inside Quit");
        //PlayerData.player.currentLevel++;
        Level.currentLevel = 0;
        Level.score = 0;
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator SlowMenu()
    {
        Debug.Log("Inside SlowMenu");
        
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
            //if (i == 0 || i % 2 == 1) { yield return new WaitForSeconds(2f); }
            //else if (i == 2) { yield return new WaitForSeconds(3f); }
            //else if (i % 2 == 0) { yield return new WaitForSeconds(5f); }
            //if(i == transform.childCount - 2)
            //{
            //    EventSystem.current.SetSelectedGameObject(GameObject.Find("NextLevel"));
            //}
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

// Update is called once per frame
void Update()
    {
    }
}

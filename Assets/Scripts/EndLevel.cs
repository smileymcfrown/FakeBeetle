using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EndLevel : MonoBehaviour
{
    GameObject child;
    public Text turnsText;
    public Text bonusText;
    public Text timeText;
    public Text totalText;
    //public Button nextButton;

    void Start()
    {
    }
  
    private void OnEnable()
    {
        StartCoroutine(SlowMenu());
    }

    IEnumerator SlowMenu()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            int result = 0;
            child = transform.GetChild(i).gameObject;
            child.SetActive(true);
            
            if (child.name == "TurnsResult") 
            { 
                result = Level.turnsRemaining;
                StartCoroutine(IncrementCoroutine(result, turnsText));
            }
            else if (child.name == "BonusResult") 
            { 
                result = 350 * Level.turnsRemaining;
                Level.score += result;
                StartCoroutine(IncrementCoroutine(result, bonusText));
            }
            else if (child.name == "TimeResult")
            {
                if (Level.timeTaken >= 600)
                { result = 0; Level.score += result; }
                else 
                { result = 6000 - ((int)Level.timeTaken * 10); Level.score += result; }
                StartCoroutine(IncrementCoroutine(result, timeText));
            }
            else if (child.gameObject.name == "TotalResult")
            {
                 StartCoroutine(IncrementCoroutine(Level.score, totalText));
            }
            
            if (i == 0 || i % 2 == 1) { yield return new WaitForSeconds(2f); }
            else if (i == 2) { yield return new WaitForSeconds(3f); }
            else if (i % 2 == 0) { yield return new WaitForSeconds(5f); }
            if(i == transform.childCount - 1)
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("NextLevel"));
            }
        }
    }

private IEnumerator IncrementCoroutine(int result, Text resultText)
    {
        int startScore = 0;
        float time = 0;
        float countTime;

        if (result < 100) { countTime = 2; }
        else { countTime = 4; }
        
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

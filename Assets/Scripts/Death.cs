using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Death : MonoBehaviour
{
    GameObject child;
    public Text scoreText;

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
            child = transform.GetChild(i).gameObject;
            child.SetActive(true);

            if (child.gameObject.name == "FinalScore")
            {
                Debug.Log("Score: " + Level.score);
                if (Level.score == 0) { scoreText.text = "000000"; }
                else { StartCoroutine(IncrementCoroutine(Level.score, scoreText)); }
            }
            else if (child.gameObject.name == "TryAgain")
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("TryAgain"));
            }

            if (i == 0) { yield return new WaitForSeconds(1f); }
            else if (i == 1) { yield return new WaitForSeconds(1.5f); }
            else if (i == 2) { yield return new WaitForSeconds(2f); }
            else if (i == 3) { yield return new WaitForSeconds(1f); }
            else if (i == 4)
            {
                if (Level.score == 0) { yield return new WaitForSeconds(1f); }
                else { yield return new WaitForSeconds(3.5f); }
            }
            else { yield return new WaitForSeconds(0.75f); }


        }
        yield break;
    }

    private IEnumerator IncrementCoroutine(int result, Text resultText)
    {
        int startScore = 0;
        float time = 0;
        float countTime = 3;

        resultText.text = startScore.ToString();
        if (result > startScore)
        {
            while (time < countTime)
            {
                yield return null;
                time += Time.deltaTime;
                float factor = time / countTime;
                resultText.text = ((int)Mathf.Lerp(startScore, result, factor)).ToString();
            }
        }
        else { yield break; }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
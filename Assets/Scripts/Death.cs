using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    GameObject child;
    public Text scoreText;

    // Script runs when Death UI Panel is activated and loads each menu item one at a time.
    private void OnEnable()
    {
        StartCoroutine(SlowMenu());
    }

    // Reset variables and either return to main menu or start game again
    public void Leave(bool restart)
    {
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
        // Cycle through child elements of Death panel
        for (int i = 0; i < transform.childCount; ++i)
        {
            //Populate variable 'child' with each element in the menu and set it active. 
            child = transform.GetChild(i).gameObject;
            child.SetActive(true);

            // Check if final score is zero (first level death) or not to adjust delay before next element.
            if (child.gameObject.name == "FinalScore")
            {
                if ( Level.score == 0) { scoreText.text = "000000"; } // PlayerData.player.score if I get it working (I won't)
                else { StartCoroutine(IncrementCount(Level.score, scoreText)); }
            }
            // Once the menu buttons are set active, make the default 'Try Again'
            else if (child.gameObject.name == "TryAgain")
            {
                EventSystem.current.SetSelectedGameObject(GameObject.Find("TryAgain"));
            }

            // Setting menu delays to allow for the counting of numbers (or not) from the second coroutine because I
            // haven't done enough study of subcoroutines to see how to pause the parent while it runs.
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


        yield break;  // Is this necessary? I would like to know.
    }

    // SubCoroutine to show points increasing from 0 to the result
    private IEnumerator IncrementCount(int result, Text resultText)
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

        else { yield break; } // Again, is this necessary?
    }
}
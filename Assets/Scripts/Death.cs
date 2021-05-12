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

    // Either return to main menu or reset variables and start game again
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
        for (int i = 0; i < transform.childCount; ++i)
        {
            child = transform.GetChild(i).gameObject;
            child.SetActive(true);

            if (child.gameObject.name == "FinalScore")
            {
                Debug.Log("Score: " + //PlayerData.player.
                    Level.score);
                if ( //PlayerData.player
                    Level.score == 0) { scoreText.text = "000000"; }
                else { StartCoroutine(IncrementCount(//PlayerData.player.
                    Level.score, scoreText)); }
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
                if (//PlayerData.player
                    Level.score == 0) { yield return new WaitForSeconds(1f); }
                else { yield return new WaitForSeconds(3.5f); }
            }
            else { yield return new WaitForSeconds(0.75f); }


        }
        yield break;
    }

    // Coroutine to make points counts up from zero
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
        else { yield break; }
    }
}
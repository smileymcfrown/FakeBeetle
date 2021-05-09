using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject objectsPrefab;
    public GameObject playerPrefab;
    public GameObject endPanel;
    public GameObject deathPanel;
    public Sprite[] spriteList;
    public Sprite[] gateList;
    public Sprite[] backgroundList;
    public Text turnsText;
    public Text scoreText;

    public static int currentLevel = 0;
    public static int score = 0;
    public static int turnsRemaining;
    public static float startTime;
    public static float timeTaken;
    public static bool gotMask = false;
    public static bool gotSnack = false;
    public static bool paused = false;

    Vector3 startPos;
    Vector3 currentPos;
    Sprite loadSprite;
    
    string[,] layout = new string[13, 9]    
    {
        { "player",      "empty" , "rubbish_bin" ,   "empty" , "luggage_02" ,    "empty" ,       "rubbish_bin" ,   "empty" , "empty" } ,
        { "empty",      "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" },
        { "empty",      "empty" , "snack_machine" , "empty" , "empty" ,         "empty" ,       "luggage_06" ,    "empty" , "mask_dispenser" },
        { "empty",      "empty" , "barrier_left" ,  "empty" , "barrier_single" ,"empty" ,       "barrier_right" , "empty" , "empty" },
        { "empty",      "empty" , "luggage_04" ,    "empty" , "empty" ,         "empty" ,       "luggage_01" ,    "empty" , "empty" },
        { "empty",      "empty" , "empty" ,         "empty" , "empty" ,         "metal_detector" ,"empty" ,       "empty" , "empty"  },
        { "luggage_01", "empty" , "barrier_left" ,  "empty" , "plant_02" ,      "empty" ,       "empty",          "empty" , "luggage_03" },
        { "empty",      "empty" , "barrier_single" ,"empty" , "empty" ,         "metal_detector" ,"empty" ,       "empty" , "empty" },
        { "luggage_01", "empty" , "barrier_right" , "empty" , "plant_02" ,      "empty" ,       "empty" ,         "empty" , "luggage_03"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" } ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "plant_01"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty",        "empty" ,         "empty" , "empty"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" }};
    // public GameObject[,] objects = new GameObject[7, 9];

    // Start is called before the first frame update

 
void Start()
    {
        startPos = new Vector3(-2.99f, -2.95f, 0f);
        LoadSave.Load();
        LevelData.openLevel = LoadSave.savedLevels[currentLevel];
        if (LevelData.openLevel.turns == 0) { turnsRemaining = 66; }
        else { turnsRemaining = LevelData.openLevel.turns; }

        // Might not be needed if loading scene anew when starting next level.
        turnsText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);

        turnsText.text = string.Format("Turns: {0}", turnsRemaining.ToString("D3"));
        

        // Just code to show the objects in the array
        string levelArray = "Objects ";
        for (int x = 0; x < LevelData.openLevel.layout.GetLength(0); ++x)
        {
            for (int y = 0; y < LevelData.openLevel.layout.GetLength(1); ++y)
            {
                levelArray = levelArray + LevelData.openLevel.layout[x,y] + " , ";
            }
        }
        Debug.Log(levelArray);

        /*
         * Need to put code for switching the background/environment image here.
         */


        if (LevelData.openLevel.levelName != "")
        {
            // Go through level array and populate cells
            for (int x = 0; x < LevelData.openLevel.layout.GetLength(0); ++x)
            {
                for (int y = 0; y < LevelData.openLevel.layout.GetLength(1); ++y)
                {
                    // Adjust physical object position to match array position
                    currentPos = startPos + new Vector3(x / 2, y / 2, 0);

                    // Skip call if empty otherwise add player sprite, then gate sprite, the all others
                    if (LevelData.openLevel.layout[x, y] != "empty")
                    {
                        if (LevelData.openLevel.layout[x, y] == "player")
                        {
                            GameObject player = Instantiate(playerPrefab, currentPos, Quaternion.identity);
                            player.name = "player";
                            player.GetComponent<SpriteRenderer>().sortingOrder = 119;
                        }
                        else if (LevelData.openLevel.layout[x, y].Contains("gate"))
                        {
                            Debug.Log("Found Gate");
                            /* This was for getting the gate sprite from the sprite list, but now there are 3 gates
                            for (int i = 0; i < spriteList.Length; ++i)
                            {
                                if (spriteList[i].name.Contains("gate"))
                                {
                                    objectSprite = spriteList[i];
                                    Debug.Log(objectSprite.name);
                                }
                            } 
                            */

                            /* Adding one of three gate sprites
                             * Poor coding.. hacked to make levels work forcing array positions
                             * Better to create new levels that use LevelData.openLevel.gate to find correct gate
                             * Possibly use List instead of array to make retrieving gate use less code.
                            */
                            if (LevelData.openLevel.gate == "gate_straya")
                            {
                                Debug.Log("Gate Straya");
                                if (LevelData.openLevel.levelName.Contains("razil"))
                                {  loadSprite = gateList[0]; Debug.Log("Brazil Sprite" + loadSprite.name); }
                                else if (LevelData.openLevel.levelName.Contains("rance"))
                                { loadSprite = gateList[1]; Debug.Log("France Sprite" + loadSprite.name); }
                                else if (LevelData.openLevel.levelName.Contains("gypt"))
                                { loadSprite = gateList[2]; Debug.Log("Australia Sprite" + loadSprite.name); }
                            }
                            else
                            {
                                Debug.Log("Not Gate Straya");
                                for (int i = 0; i < gateList.Length; ++i)
                                {
                                    if (gateList[i].name == LevelData.openLevel.layout[x, y])
                                    {
                                        
                                        loadSprite = gateList[i];
                                        Debug.Log("Other Gate Sprite" + loadSprite.name);
                                        Debug.Log(loadSprite.name);
                                    }
                                }
                            }
                            GameObject newObject = Instantiate(objectsPrefab, currentPos + new Vector3(0, 1, 0), Quaternion.identity);
                            newObject.name = loadSprite.name;
                            newObject.GetComponent<SpriteRenderer>().sprite = loadSprite;
                            newObject.GetComponent<SpriteRenderer>().sortingOrder = 117 - ((y * 13) + x);
                        }
                        else  // Instantiate all other level objects
                        {
                            bool found = false;
                            for (int i = 0; i < spriteList.Length; ++i)
                            {
                                if (spriteList[i].name == LevelData.openLevel.layout[x, y].Substring(3))
                                {
                                    loadSprite = spriteList[i];
                                    Debug.Log(loadSprite.name);
                                    found = true;
                                }
                            }
                            if (found)
                            {
                                GameObject newObject;

                                if (loadSprite.name.Contains("_v"))  // Check for objects that are shifted up but 0.5units
                                {
                                    newObject = Instantiate(objectsPrefab, currentPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
                                }
                                else
                                {
                                    newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);

                                }
                                newObject.name = LevelData.openLevel.layout[x, y].Substring(0, 3) + loadSprite.name;
                                Debug.Log("Created: " + newObject.name);
                                newObject.GetComponent<SpriteRenderer>().sprite = loadSprite;
                                Debug.Log("Sort: " + x + "," + y + " : " + (118 - (y * 13 + x)));
                                newObject.GetComponent<SpriteRenderer>().sortingOrder = 118 - (y * 13 + x);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            /*  Temporary Code for using massive array declared above if no saved level is found.
             *  Can be deleted when finished testing and level data is saved a file
             *  that won't be deleted for sure.
             */

            // Get layout Array
            for (int x = 0; x < layout.GetLength(0); ++x)
            {
                for (int y = 0; y < layout.GetLength(1); ++y)
                {
                    if (layout[x, y] != "empty")
                    {
                        for (int i = 0; i < spriteList.Length; ++i)
                        {
                            if (spriteList[i].name == layout[x, y])
                            {
                                loadSprite = spriteList[i];
                            }

                        }
                        currentPos = startPos + new Vector3(x / 2, y / 2, 0);
                        GameObject newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);
                        newObject.name = loadSprite.name; //= objectName;
                        newObject.GetComponent<SpriteRenderer>().sprite = loadSprite;
                    }
                }
            }
        }
        startTime = Time.time;
    }

    public void UpdateHUD() 
    {
        turnsText.text = string.Format("Turns: {0}", turnsRemaining.ToString("D2"));
        scoreText.text = string.Format("{0}", score.ToString("D6"));
    }

    public void Death()
    {
        turnsText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        //Set score for testing purposes
        score = 104853;
        
        deathPanel.SetActive(true);
    }

    public void LevelComplete()
    {
        //GameObject turns = GameObject.Find("TurnsResult");
        //GameObject turnBonus = GameObject.Find("TurnsResult");
        //GameObject timeBonus = GameObject.Find("TimeResult");
        //GameObject total = GameObject.Find("TotalResult");
        //float timeDelay = 2f;
        turnsText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        paused = true;
        Debug.Log("Scale: " + Time.timeScale);
        endPanel.SetActive(true);
        

    }
    // Update is called once per frame
    void Update()
    {

    }
}

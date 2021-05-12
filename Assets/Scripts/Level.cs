using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject objectsPrefab;
    public GameObject playerPrefab;
    public GameObject detectorPrefab;
    public GameObject endPanel;
    public GameObject deathPanel;
    //public GameObject gameCompletePanel;
    public Sprite[] spriteList;
    public Sprite[] gateList;
    public Sprite[] backgroundList;
    public Text turnsText;
    public Text scoreText;
    public static int currentLevel = 0;
    public static int score = 0;
    public static bool paused = false;
    public static int turnsRemaining;
    public static float gameStartTime;
    public static float levelStartTime;
    public static float timeTaken;
    public static float totalTime;

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
        /* Uncomment if loading Level.Scene directly instead of playing through MainMenu.Scene */
        LoadSave.Load();

        /* Attempted to create a PlayerData class to hold player score and current level.. It failed. */
        //PlayerData.player = new PlayerData();
        //PlayerData.player.score = 0;

        // Forcing levels to load correctly because I haven't implemented saving the levels into the List<>
        // correctly. This way the correct level order occurs regardless of position in the List<>
        bool foundLevel = false;
        foreach (LevelData level in LoadSave.savedLevels)
        {
            Debug.Log("foreach: " + level.levelName);

            if (currentLevel == 0 && level.levelName.Contains("razil"))
            {
                Debug.Log("Level: Brazil");
                LevelData.openLevel = level;
                foundLevel = true;
                break;
            }
            else if (currentLevel == 1 && level.levelName.Contains("rance"))
            {
                Debug.Log("Level: France");
                LevelData.openLevel = level;
                foundLevel = true;
                break;
            }
            else if (currentLevel == 2 && level.levelName.Contains("gypt"))
            {
                Debug.Log("Level: Egypt");
                LevelData.openLevel = level;
                foundLevel = true;
                break;
            }
        }
        if (!foundLevel)
        {
            Debug.Log("FAIL - Default Level Loaded!");
            LevelData.openLevel = LoadSave.savedLevels[0]; //PlayerData.player.currentLevel];
        }


        // ######### Just code to show the objects in the array... can be deleted after testing.
        string levelArray = "Objects ";
        for (int x = 0; x < LevelData.openLevel.layout.GetLength(0); ++x)
        {
            for (int y = 0; y < LevelData.openLevel.layout.GetLength(1); ++y)
            {
                levelArray = levelArray + LevelData.openLevel.layout[x, y] + " , ";
            }
        }
        Debug.Log(levelArray);


        // Make sure game is not paused and starting coordinates set.
        paused = false;
        startPos = new Vector3(-2.99f, -2.95f, 0f);

        // Set number of turns for the current level
        turnsRemaining = LevelData.openLevel.turns;


        // ######### Might not be needed if loading scene anew when starting next level?
        turnsText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);


        // Update HUD with Turns
        UpdateHUD();

        // Change the background to match the level
        GameObject.Find("Level").GetComponent<SpriteRenderer>().sprite = backgroundList[currentLevel];

        // If Levels don't exist, then most likely, the levels.ld file in Assests/StreamingAssets folder is missing!
        // Otherwise there is an error somewhere and the level will be populated with a default level declared
        // in the massive array at the top of this script.
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
                            if (x == 12)
                            {
                                player.GetComponent<SpriteRenderer>().sortingOrder = 107;
                                player.GetComponent<SpriteRenderer>().flipX = true;
                            }
                            else 
                            {
                                player.GetComponent<SpriteRenderer>().sortingOrder = 119;
                            }
                        }
                        
                        if (LevelData.openLevel.layout[x, y].Contains("detector"))
                        {
                            GameObject detector = Instantiate(detectorPrefab, currentPos + new Vector3(0,0.5f,0), Quaternion.identity);
                            detector.name = "detector_v";
                            detector.GetComponent<SpriteRenderer>().sortingOrder = 117 - ((y * 13) + x);
                        }
                        else if (LevelData.openLevel.layout[x, y].Contains("gate"))
                        {
                            Debug.Log("Found Gate");
                            /* Adding one of three gate sprites
                             * Poor coding.. hacked to make levels work forcing array positions
                             * Better to create new levels that use LevelData.openLevel.gate to find correct gate
                             * Possibly use List instead of array to make retrieving gate use less code.
                            */
                            if (LevelData.openLevel.gate == "gate_straya")
                            {
                                Debug.Log("Gate Straya");
                                if (LevelData.openLevel.levelName.Contains("razil"))
                                {  loadSprite = gateList[0]; Debug.Log("Brazil Sprite " + loadSprite.name); }
                                else if (LevelData.openLevel.levelName.Contains("rance"))
                                { loadSprite = gateList[1]; Debug.Log("France Sprite " + loadSprite.name); }
                                else if (LevelData.openLevel.levelName.Contains("gypt"))
                                { loadSprite = gateList[2]; Debug.Log("Australia Sprite " + loadSprite.name); }
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
                                if (!loadSprite.name.Contains("detector"))
                                {
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
        }


        // #########
        /* Temporary Code for using massive array declared at top if no saved level is found.
         * Can be deleted when finished testing and level data is saved to a file
         * that won't be deleted for sure.
         */
        else
        {
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


        // Set the start time for the game and the level
        if (currentLevel == 0) { gameStartTime = Time.time; levelStartTime = Time.time; }
        else { levelStartTime = Time.time; }
    }

    // Update Turns and Score HUD
    public void UpdateHUD() 
    {
        turnsText.text = string.Format("Turns: {0}", turnsRemaining.ToString("D2"));
        scoreText.text = string.Format("{0}", score.ToString("D6")); //PlayerData.player.score.ToString("D6"));
    }

    // Hide HUD and show Death UI
    public void Death()
    {
        turnsText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        deathPanel.SetActive(true);
    }

    // Hide HUD and show Level Complete UI
    public void LevelComplete()
    {
        turnsText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        paused = true;
        // Time.timeScale = 0;

        {
            endPanel.SetActive(true);
        }


    }

    // Hide HUD and show Game Complete UI
    public void GameComplete()
    {
        //if (//PlayerData.player.
        //currentLevel == 2)
        //{
        endPanel.SetActive(false);
       // gameCompletePanel.SetActive(true);
        //}
        //else
    }
    
}

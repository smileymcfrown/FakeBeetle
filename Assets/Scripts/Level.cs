using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject objectsPrefab;
    public GameObject playerPrefab;
    public Sprite[] spriteList;
    Vector3 startPos;
    Vector3 currentPos;
    Sprite objectSprite;
    int currentLevel = 0;
    
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
        startPos = new Vector3(-2.7f, -2.95f, 0f);
        LoadSave.Load();
        LevelData.openLevel = LoadSave.savedLevels[0];

        // Just code to show the objects in the array
        string levelArray = "Objects ";
        for (int a = 0; a < LevelData.openLevel.layout.GetLength(0); ++a)
        {
            for (int q = 0; q < LevelData.openLevel.layout.GetLength(1); ++q)
            {
                levelArray = levelArray + LevelData.openLevel.layout[a,q] + " , ";
            }
        }
        Debug.Log(levelArray);



       if (LevelData.openLevel.levelName != "")
        {
            for (int x = 0; x < LevelData.openLevel.layout.GetLength(0); ++x)
            {
                for (int y = 0; y < LevelData.openLevel.layout.GetLength(1); ++y)
                {
                    currentPos = startPos + new Vector3(x / 2, y / 2, 0);

                    if (LevelData.openLevel.layout[x, y] != "empty")
                    {
                        if (LevelData.openLevel.layout[x, y] == "player")
                        {
                            GameObject player = Instantiate(playerPrefab, currentPos, Quaternion.identity);
                            player.name = "player";
                            player.GetComponent<SpriteRenderer>().sortingOrder = 11;
                        }
                        else if (LevelData.openLevel.layout[x, y] == "gate")
                        {
                            for (int i = 0; i < spriteList.Length; ++i)
                            {
                                if (spriteList[i].name.Contains("gate"))
                                {
                                    objectSprite = spriteList[i];
                                    Debug.Log(objectSprite.name);
                                }
                            }
                            GameObject newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);
                            newObject.name = objectSprite.name;
                            newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
                            newObject.GetComponent<SpriteRenderer>().sortingOrder = 10 - y;
                        }
                        else
                        {
                            bool found = false;
                            for (int i = 0; i < spriteList.Length; ++i)
                            {
                                if (spriteList[i].name == LevelData.openLevel.layout[x, y].Substring(3))
                                {
                                    objectSprite = spriteList[i];
                                    Debug.Log(objectSprite.name);
                                    found = true;
                                }
                            }
                            if (found)
                            {
                                GameObject newObject;
                            
                                if (objectSprite.name.Contains("_v"))
                                {
                                    newObject = Instantiate(objectsPrefab, currentPos + new Vector3(0, 0.5f, 0), Quaternion.identity);
                                }
                                else
                                {
                                    newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);

                                }
                                newObject.name = LevelData.openLevel.layout[x, y].Substring(0, 3) + objectSprite.name;
                                newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
                                newObject.GetComponent<SpriteRenderer>().sortingOrder = 10 - y;
                            }
                        }
                    }
                }
            }
        }
        else {
            /*  Working Code for using above array instead of loading from Saved Data
             *  
             *  */

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
                                objectSprite = spriteList[i];
                            }

                        }
                        currentPos = startPos + new Vector3(x / 2, y / 2, 0);
                        GameObject newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);
                        newObject.name = objectSprite.name; //= objectName;
                        newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
                    }
                }
            }
        }
        
    }

    public void LevelComplete()
    {
        // Some code to finish the level, update turns remaining, update points,
        // and update currentLevel to go to the next level in whatever array someone makes
        // Then load the level complete screen.

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

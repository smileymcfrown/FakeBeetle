using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditor : MonoBehaviour
{

    // Create variables to save level data

    // string[,] layout = new string[13, 9];
    // public string levelName;
    // public int turns;
    // public string background;



    // Variables used for scrolling through object sprites
    // before selecting. Maybe combine all sprites into one button? Separate by name?
    public Sprite[] icons;
    public Sprite[] objects;
    public Sprite playerSprite;
    public Sprite gateSprite;
    public Button objectsButton;
    public Toggle toggleStart;
    public Toggle toggleGate;
    int objectsPos = 0; //Which object is currently displayed in the list
    string selectedMenu;  // To store the name of the currently selected menu item.
    GameObject lastSelected; // To return focus to menu if mouse is clicked elsewhere

    // Save and Load Variables
    public InputField inputName, inputTurns;
    public Image startImage, gateImage, objectsLeft, objectsRight;
    


    // Variables used to instantiate chosen object at default position with object's sprite name
    public GameObject objectsPrefab; //For items that can be moved around
    public GameObject togglePrefab; // For items that toggle between two locations
    Vector3 placeOrigin;  // Default location for objects to appear on the grid
    int objectID = 10;

 
    GameObject player, gate; // Used to instantiate the toggle objects on start
 
    bool gateRight = true; // For toggling two locations
    bool startLeft = true; //

    // bool placing = false;
 
    void Start()
    {
        LevelData.openLevel = new LevelData(); // clear openLevel to start fresh
        placeOrigin = new Vector3(-2f, 0.3f, 0); // Default coordinates to instantiate objects in centre of grid

        for (int i = 0; i < LevelData.openLevel.layout.GetLength(0); i++) // Populate grid positions with default "empty"
        { for (int j = 0; j < LevelData.openLevel.layout.GetLength(1); j++) { LevelData.openLevel.layout[i, j] = "empty"; } }

        // Instantiate player object immediately at default position with highest sorting order
        startLeft = true;
        Vector3 newPos = placeOrigin + new Vector3(-3, -2, 0);
        player = Instantiate(togglePrefab, newPos, Quaternion.identity);
        player.name = playerSprite.name;
        player.GetComponent<SpriteRenderer>().sprite = playerSprite;
        LevelData.openLevel.layout[0, 0] = player.name;
        player.GetComponent<SpriteRenderer>().sortingOrder = 11;

        // Instantiate gate object immediately at default position with lowest sorting order
        gateRight = true;
        newPos = placeOrigin + new Vector3(3, 2.8f, 0);
        gate = Instantiate(togglePrefab, newPos, Quaternion.identity);
        gate.name = gateSprite.name;
        gate.GetComponent<SpriteRenderer>().sprite = gateSprite;
        LevelData.openLevel.layout[12, 8] = gate.name;
        gate.GetComponent<SpriteRenderer>().sortingOrder = 2;

        //Not sure why but object button displays wrong object on start
        // objectsButton.GetComponent<Image>().sprite = objects[objectsPos];

        // Code I used to check how vectors work - keep until player/object movement is fixed.
        Vector3 pos = new Vector3(3, 5, 5);
        Vector3 movePos = new Vector3(3, 4, 5);
        Vector3 pos2 = new Vector3(3, -2, 5);
        Vector3 movePos2 = new Vector3(3, -3, 5);

        Vector3 result = movePos - pos;
        Vector3 result2 = movePos - pos;
        Vector3 show = movePos + result;
        Vector3 show2 = movePos2 + result2;
        Debug.Log("Result: "+ show + " : " + show2);

    }

    // Update is called once per frame
    void Update()
    {
        
        // This code was for setting if the generated object should be offset or not.
        // ** Possibly obsolete. **

        /* if (EventSystem.current.currentSelectedGameObject.name == "Grid")
         * { }
         * else { 
         * if(EventSystem.current.currentSelectedGameObject.name == "btnSpecial")
         * { barriers = true; }
         * else if (EventSystem.current.currentSelectedGameObject.name == "btnObjects")
         * { barriers = false; }
         * else { 
         */
        
        //Stop mouse clicks removing focus from the menu by always returning to last selected is nothing is selected.
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
            selectedMenu = EventSystem.current.currentSelectedGameObject.name;
        }
        else
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
            selectedMenu = EventSystem.current.currentSelectedGameObject.name;
         }

        // Change highlighted menu items because toggles needed two different menu-item-selected sprites, one
        // for each toggle state, and I could figure out how to do that. Instead, toggle label image is changed.
        // Also, left and right arrow images need to change when btnObjects button is selected.
        if (selectedMenu == "btnObjects")
        {
            startImage.sprite = icons[0];
            gateImage.sprite = icons[2];
            objectsLeft.sprite = icons[6];
            objectsRight.sprite = icons[7];
        }
        else if (selectedMenu == "toggleStart")
        {
            
            startImage.sprite = icons[1];
            gateImage.sprite = icons[2];
            objectsLeft.sprite = icons[4];
            objectsRight.sprite = icons[5];
        }
        else if (selectedMenu == "toggleGate")
        {
            startImage.sprite = icons[0];
            gateImage.sprite = icons[3];
            objectsLeft.sprite = icons[4];
            objectsRight.sprite = icons[5];
        }
        else 
        {
            startImage.sprite = icons[0];
            gateImage.sprite = icons[2];
            objectsLeft.sprite = icons[4];
            objectsRight.sprite = icons[5];
        }

        // Left right arrows to change object to select or to change toggle state.
        // Maybe should change to use Input.GetAxisRaw the same as for player movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectLeftRight(selectedMenu, true);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectLeftRight(selectedMenu, false);
        }
    }


    public void ToggleStart()
    {
        if (startLeft)
        {
            if (LevelData.openLevel.layout[12, 0] != "empty")
            {
                //Pop up a warning about object be deleted by switching positions
                Debug.Log("You have removed the thing that was where this is now.");
            }

            player.transform.position += new Vector3(6, 0, 0);
            player.GetComponent<SpriteRenderer>().flipX = !player.GetComponent<SpriteRenderer>().flipX;
            LevelData.openLevel.layout[12, 0] = "player";
            LevelData.openLevel.layout[0, 0] = "empty";
            startLeft = false;
        }
        else
        {
            if (LevelData.openLevel.layout[0, 0] != "empty")
            {
                //Pop up a warning about object be deleted by switching positions
                Debug.Log("You have removed the thing that was where this is now.");
                
            }
            player.transform.position += new Vector3(-6, 0, 0);
            player.GetComponent<SpriteRenderer>().flipX = !player.GetComponent<SpriteRenderer>().flipX;
            LevelData.openLevel.layout[0, 0] = "player";
            LevelData.openLevel.layout[12, 0] = "empty";
            startLeft = true;
        }
    }

    public void ToggleGate()
    {
        if (gateRight)
        {
            if (LevelData.openLevel.layout[0, 8] != "empty")
            {
                //Pop up a warning about object be deleted by switching positions
                Debug.Log("You have removed the thing that was where this is now.");
            }

            gate.transform.position += new Vector3(-6, 0, 0);
            LevelData.openLevel.layout[0, 8] = "gate";
            LevelData.openLevel.layout[12, 8] = "empty";
            gateRight = false;
        }
        else
        {
            if (LevelData.openLevel.layout[12, 8] != "empty")
            {
                //Pop up a warning about object be deleted by switching positions
                Debug.Log("You have removed the thing that was where this is now.");
            }
            gate.transform.position += new Vector3(6, 0, 0);
            LevelData.openLevel.layout[12, 8] = "gate";
            LevelData.openLevel.layout[0, 8] = "empty";
            gateRight = true;
            
        }

    }

    public void SelectLeftRight(string name, bool left)
    {
        Debug.Log(name + " - Left: " + left);

        // Check menu item selected and either cycle object sprites or change toggle state
        if(name == "btnObjects")
        {
            // Continuously cycle through object sprite array
            if (objectsPos == 0 && left) 
            { objectsPos = objects.Length - 1; }
            else if (objectsPos == objects.Length - 1 && !left) 
            { objectsPos = 0; }
            else { if (left) { objectsPos--; } else { objectsPos++; } }

            // Change to next sprite
            objectsButton.GetComponent<Image>().sprite = objects[objectsPos];

            // Tried to make left/right arrows change sprite on keypress; however, it's so quick it's pointless.
            objectsLeft.sprite = icons[8];
        }
        else if(name == "toggleStart")
        {
            if (left) { toggleStart.isOn = true; }
            else { toggleStart.isOn = false; }
        }
        else if (name == "toggleGate")
        {
            if (left) { toggleGate.isOn = false; }
            else { toggleGate.isOn = true; }
        }
    }

    // PlaceObject instantiates selected object on the grid at the default position
    public void PlaceObject()
    {
        
        Sprite placeSprite = objectsButton.GetComponent<Image>().sprite;
        GameObject newObject;

        if (placeSprite.name.Contains("_v"))
            { newObject = Instantiate(objectsPrefab, placeOrigin + new Vector3(0, 0.5f, 0), Quaternion.identity); }
        else 
            { newObject = Instantiate(objectsPrefab, placeOrigin, Quaternion.identity); }

        objectID++;
        newObject.name = objectID + "_" +  placeSprite.name;
        newObject.GetComponent<SpriteRenderer>().sprite = placeSprite;
    }

    // Add object name at chosen position to the level layout 2D array.
    public void AddObject(int posX, int posY, string name)
    {
        Debug.Log("AddObject: " + posX + "," + posY + ": " + name);
        
        if (LevelData.openLevel.layout[posX, posY] == "empty")
        {
            if (name.Contains("detector")) { LevelData.openLevel.layout[posX - 2, posY - 1] = "x-ray"; }
            else if(name.Contains("tile"))
            {
                LevelData.openLevel.layout[posX, posY] = "empty";
                
        
            }
            LevelData.openLevel.layout[posX, posY] = name;
            Debug.Log(LevelData.openLevel.layout[posX, posY]);
        }

        // What to do if it's not empty?!?!
    }

    // Add turns and level name to openLevel then calls LoadSave.Save() to save to a file.
    public void SaveLayout()
    {
        // Make sure the is a name for this level.
        if (inputName.text != null || inputTurns.text != null)
        {
            // Possibly add code to check if no objects have been placed with error message
            // advising empty level cannot be saved.

            // ** Check to see if name is already used and give error if so.
            // or maybe just check if it's one of the 3 main levels and give an error otherwise overwrite.
            if(inputName.text.Contains("razil"))
            { LevelData.openLevel.background = "1st_level_brazil"; }
            else if(inputName.text.Contains("rance"))
            { LevelData.openLevel.background = "2nd_level_france"; }
            else if (inputName.text.Contains("gypt"))
            { LevelData.openLevel.background = "3rd_level_egypt"; }

            /* Uncomment after including a list of gates to choose from when creating levels.
             * Until then gate_straya is the default when LevelData.openLevel is created.
             * Or add a sprite font to use on a blank, generic gate and dynamically change
             * gate name to that of next level.
             * 
             * if (gate != "gate_straya") { LevelData.openLevel.gate = inputGate.text }
             */

            LevelData.openLevel.levelName = inputName.text;

            LevelData.openLevel.turns = int.Parse(inputTurns.text);
            Debug.Log(LevelData.openLevel.levelName);
            LoadSave.Save();
            // ** Update Saved Level List with new name
        }
        else
        {
            // ** This code should be changed to display an error message to the player.
            Debug.Log("You must enter a name to save the level");
        }
        
    }

    // Back to Main Menu
    public void QuitEditor()
    {
        SceneManager.LoadScene("MainMenu");
    }


    // Loads a saved level from the list into the editor.. NOT WORKING!
    public void LoadLayout(int levelIndex)
    {
            // ** Load item chosen from the list
            
            // Get level data from the list of levels and put it in "openLevel"
            LevelData.openLevel = LoadSave.savedLevels[levelIndex];
            
            // All level data should now be accessible from whatever script using
            // LevelData.openLevel.(levelName, layout[,], turns, background
    }
 
    // Test function to check AddObject is working.. can be deleted after testing!
    public void CheckObject(int posX, int posY)
    {
        string returnSelect = EventSystem.current.currentSelectedGameObject.name;
        placeOrigin = new Vector3((posX * .5f), (posY * .5f) + 1f, 0);
        //string objName = LevelData.openLevel.layout[posX, posY];
        //Debug.Log(objName);

        Sprite placeSprite = objects[0];

        Debug.Log("ObjName: " + objects[0].name);
        Debug.Log("PlaceName: " + LevelData.openLevel.layout[posX, posY]);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name == LevelData.openLevel.layout[posX, posY])
            {
                placeSprite = objects[i];
            }

        }
        Debug.Log("placeSprite: " + placeSprite.name);
        //GameObject newObject = Instantiate(objectsPrefab, placeOrigin, Quaternion.identity);
        //newObject.name = placeSprite.name; //= objectName;
        //newObject.GetComponent<SpriteRenderer>().sprite = placeSprite;



        //int index = ArrayUtility.IndexOf(objects, objName);
        //Debug.Log(index);

        // Sprite placeSprite = objects[index];
        //GameObject checker = Instantiate(objectsPrefab, checkerPos, Quaternion.identity);
        //checker.name = "CHEKER_"+ placeSprite.name;
        //checker.GetComponent<SpriteRenderer>().sprite = placeSprite;
        EventSystem.current.SetSelectedGameObject(GameObject.Find(returnSelect));
    }
}

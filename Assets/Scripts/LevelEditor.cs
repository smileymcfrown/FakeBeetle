using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{

    // Create variables to save level data
    
    // string[,] layout = new string[13, 9];
    // public string levelName;
    // public int turns;
    // public string background;
    


    // Variables used for scrolling through object sprites
    // before selecting. Maybe combine all sprites into one button? Separate by name?
    public Sprite[] special;
    public Sprite[] objects;
    public Sprite playerSprite;
    public Sprite gateSprite;
    public Button specialButton;
    public Button objectsButton;
    int specialPos; //Which object is currently displayed in the list
    int objectsPos; //Ditto
    string selectedMenu;  // To store the name of the currently selected menu item.

    //Save and Load Variables
    public InputField inputName;


    // Variables used to instantiate chosen object at default position with object's sprite name
    public GameObject objectsPrefab; //For items that can be moved around
    public GameObject togglePrefab; // For items that toggle between two locations
    Vector3 placeOrigin;  // Default location for objects to appear on the grid
    int objectID = 10;

 
    GameObject player, gate; // Used to instantiate the toggle objects on start
 
    bool gateRight = true; // For toggling two locations
    bool startLeft = true; //

    //bool placing = false;
 
    void Start()
    {
        LevelData.openLevel = new LevelData();
        placeOrigin = new Vector3(-2f, 0, 0);

        for (int i = 0; i < LevelData.openLevel.layout.GetLength(0); i++)
        { for (int j = 0; j < LevelData.openLevel.layout.GetLength(1); j++) { LevelData.openLevel.layout[i, j] = "empty"; } }

            //specialPrefab.SetActive(true);
            //specialRenderer.sprite = special[specialPos];
            //objectsRenderer.sprite = objects[objectsPos];

       // Debug.Log(placeOrigin);
        Vector3 newPos = placeOrigin + new Vector3(-3, -2, 0);
        // Debug.Log(newPos);

        player = Instantiate(togglePrefab, newPos, Quaternion.identity);
        player.name = playerSprite.name;
        player.GetComponent<SpriteRenderer>().sprite = playerSprite;
        LevelData.openLevel.layout[0, 0] = player.name;
        player.GetComponent<SpriteRenderer>().sortingOrder = 11;

        newPos = placeOrigin + new Vector3(3, 3, 0);
        gate = Instantiate(togglePrefab, newPos, Quaternion.identity);
        gate.name = gateSprite.name;
        gate.GetComponent<SpriteRenderer>().sprite = gateSprite;
        LevelData.openLevel.layout[12, 8] = gate.name;
        gate.GetComponent<SpriteRenderer>().sortingOrder = 2;


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
        // Possibly obsolete.

        /* if (EventSystem.current.currentSelectedGameObject.name == "Grid")
        { }
        else { 
        if(EventSystem.current.currentSelectedGameObject.name == "btnSpecial")
        { barriers = true; }
        else if (EventSystem.current.currentSelectedGameObject.name == "btnObjects")
        { barriers = false; }
        else
        { */


        selectedMenu = EventSystem.current.currentSelectedGameObject.name;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectLeft(selectedMenu);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectRight(selectedMenu);
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
            LevelData.openLevel.layout[12, 0] = player.name;
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
            LevelData.openLevel.layout[0, 0] = player.name;
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
            LevelData.openLevel.layout[0, 8] = gate.name;
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
            LevelData.openLevel.layout[12, 8] = gate.name;
            LevelData.openLevel.layout[0, 8] = "empty";
            gateRight = true;
            
        }

    }

    public void SelectLeft(string name)
    {
        Debug.Log("Left: " + name);
        if (name == "btnSpecial")
        {
            if (specialPos == 0) { specialPos = special.Length - 1; }
            else { specialPos--; }

            specialButton.GetComponent<Image>().sprite = special[specialPos];

        }
        else if(name == "btnObjects")
        {
            if (objectsPos == 0) { objectsPos = objects.Length - 1; }
            else { objectsPos--; }
            objectsButton.GetComponent<Image>().sprite = objects[objectsPos];
        }
    }

    public void SelectRight(string name)
    {
        Debug.Log("Right: " + name);
        if (name == "btnSpecial")
        {
            if (specialPos == special.Length - 1) { specialPos = 0; }
            else { specialPos++; }
            specialButton.GetComponent<Image>().sprite = special[specialPos];
        }
        else if (name == "btnObjects")
        {
            if (objectsPos == objects.Length - 1) { objectsPos = 0; }
            else { objectsPos++; }
            objectsButton.GetComponent<Image>().sprite = objects[objectsPos];
        }
    }

    /* Keeping old code of PlaceSpecial just in case until testing complete. Code combined into PlaceObject below.
        public void PlaceSpecial()
    {
        // Sprite placeSprite = specialRenderer.sprite;
        Sprite placeSprite = specialButton.GetComponent<Image>().sprite;
        objectName = placeSprite.name;
        GameObject newObject = Instantiate(objectsPrefab, placeOrigin, Quaternion.identity);
        newObject.name = objectName;
        //newObject.GetComponent<>().objectName = objectName;
        newObject.GetComponent<SpriteRenderer>().sprite = placeSprite;
        //objectRenderer.sprite = special[specialPos]; 
        //Instantiate(specialPrefab, specialOrigin, Quaternion.identity);
        //placing = true;
    } */

    // PlaceObject instantiates selected object on the grid at the default position
    public void PlaceObject()
    {
        
        Sprite placeSprite = objectsButton.GetComponent<Image>().sprite;
        GameObject newObject;
        
        if (placeSprite.name.Contains("_v"))
            { newObject = Instantiate(objectsPrefab, placeOrigin + new Vector3(0,0.5f,0), Quaternion.identity); }
        else
            { newObject = Instantiate(objectsPrefab, placeOrigin, Quaternion.identity); }

        objectID++;
        newObject.name = objectID + "_" +  placeSprite.name;
        newObject.GetComponent<SpriteRenderer>().sprite = placeSprite;




    }

    // Add object name at chosen position to the level layout 2D array.
    public void AddObject(int posX, int posY, string name)
    {
        Debug.Log("AddObject: "+posX+","+posY+": "+name);
        LevelData.openLevel.layout[posX, posY] = name;
        Debug.Log(LevelData.openLevel.layout[posX, posY]);
    }

    public void SaveLayout()
    {
        // Make sure the is a name for this level.
        if (inputName.text != null)
        {
            // ** Check to see if name is already used and give error if so.
            // or maybe just check if it's one of the 3 main levels and give an error otherwise overwrite.

            LevelData.openLevel.levelName = inputName.text;
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
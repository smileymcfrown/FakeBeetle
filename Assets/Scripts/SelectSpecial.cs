using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSpecial : MonoBehaviour
{

    //public GameObject specialPrefab;
    public GameObject objectsPrefab;
    public Sprite[] special;
    public Sprite[] objects;
    //public SpriteRenderer specialRenderer;
    public Button specialButton;
    public Button objectsButton;
    //public SpriteRenderer objectsRenderer;
    public int specialPos;
    public int objectsPos;
    public Vector3 placeOrigin;
    public string objectName;

    public string[,] layout = new string[13,9];
    public GameObject[,] layoutAll = new GameObject[13,9];

    bool barriers;
    //bool placing = false;
    // [SerializeField] private GameObject specialButton;

    // Start is called before the first frame update
    void Start()
    {
        //specialPrefab.SetActive(true);
        //specialRenderer.sprite = special[specialPos];
        //objectsRenderer.sprite = objects[objectsPos];
       
        Debug.Log("START");
    }

    public void SelectLeft(string name)
    {
        Debug.Log("Left: " + name);
        if (name == "btnSpecial")
        {
            if (specialPos == 0) { specialPos = special.Length - 1; }
            else { specialPos--; }
            //specialRenderer.sprite = special[specialPos];
            specialButton.GetComponent<Image>().sprite = special[specialPos];
            //Vector2 newSize = specialButton.GetComponent<Image>().sprite.bounds.size;
            //specialButton.GetComponent<RectTransform>().sizeDelta = newSize;
        }
        else if(name == "btnObjects")
        {
            if (objectsPos == 0) { objectsPos = objects.Length - 1; }
            else { objectsPos--; }
            //objectsRenderer.sprite = objects[objectsPos];
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
            //specialRenderer.sprite = special[specialPos];
            specialButton.GetComponent<Image>().sprite = special[specialPos];
            //Vector2 newSize = specialButton.GetComponent<Image>().sprite.bounds.size;
            //specialButton.GetComponent<RectTransform>().sizeDelta = newSize;
        }
        else if (name == "btnObjects")
        {
            if (objectsPos == objects.Length - 1) { objectsPos = 0; }
            else { objectsPos++; }
            objectsButton.GetComponent<Image>().sprite = objects[objectsPos];
            //objectsRenderer.sprite = objects[objectsPos];
        }
    }

    /* public void PlaceSpecial()
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

    public void PlaceObject(bool barrier)
    {
        Sprite placeSprite;
        if (barrier == true) 
        { 
            placeOrigin = new Vector3(-2f, 0.5f, 0);
            placeSprite = specialButton.GetComponent<Image>().sprite;

        }
        else  
        { 
            placeOrigin = new Vector3(-2f, 0, 0);
            placeSprite = objectsButton.GetComponent<Image>().sprite;

        }
        //objectName = placeSprite.name;
        GameObject newObject = Instantiate(objectsPrefab, placeOrigin, Quaternion.identity);
        newObject.name = placeSprite.name; //= objectName;
        newObject.GetComponent<SpriteRenderer>().sprite = placeSprite;
        //placing = true;
    }

    public void AddObject(int posX, int posY, string name)
    {
        Debug.Log("AddObject: "+posX+","+posY+": "+name);
        layout[posX, posY] = name;
        Debug.Log(layout[posX, posY]);
    }
    public void AddAll(int posX, int posY, GameObject placeObject)
    {
        Debug.Log("AddAll");
        layoutAll[posX, posY] = placeObject;
        Debug.Log(placeObject.name);
    }

    public void CheckAll(int posX, int posY)
    {
        string returnSelect = EventSystem.current.currentSelectedGameObject.name;
        Vector3 checkerPos = new Vector3((posX*.5f), (posY*.5f)+1, 0);
        GameObject checker = Instantiate(layoutAll[posX, posY], checkerPos, Quaternion.identity);
        checker.name = "CHEKER";
        EventSystem.current.SetSelectedGameObject(GameObject.Find("returnSelect"));
    }
    public void CheckObject(int posX, int posY)
    {
        string name;
        name = layout[posX, posY];
        Debug.Log(name);
    }

    // Update is called once per frame
    void Update()
    {
        /* if (EventSystem.current.currentSelectedGameObject.name == "Grid")
        { }
        else { 
        if(EventSystem.current.currentSelectedGameObject.name == "btnSpecial")
        { barriers = true; }
        else if (EventSystem.current.currentSelectedGameObject.name == "btnObjects")
        { barriers = false; }
        else
        { */
        if (EventSystem.current.currentSelectedGameObject.name != null)
        {
            objectName = EventSystem.current.currentSelectedGameObject.name;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectLeft(objectName);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectRight(objectName);
            }
        }

        
        /*
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        if (EventSystem.current.currentSelectedGameObject.name == "btnSpecial")
        {
            Debug.Log("Special Selected");
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SelectLeft(true);
                //EventSystem.current.SetSelectedGameObject();
                //Image buttonSprite = GameObject.Find("btnSpecial").GetComponent<Image>();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SelectRight(true);
            }

        }
        else if (EventSystem.current.currentSelectedGameObject.name == "btnObjects")
        {
            Debug.Log("Objects Selected");
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ObjectsLeft();
                //EventSystem.current.SetSelectedGameObject();
                //Image buttonSprite = GameObject.Find("btnSpecial").GetComponent<Image>();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ObjectsRight();
            }

        }
        */
    }
}

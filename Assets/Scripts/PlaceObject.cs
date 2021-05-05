using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{

    public float moveSpeed = 1.2f;
    public Vector3 movePoint;
   

    int objectPosX = 6;
    int objectPosY = 4;
    int newPosX;
    int newPosY;
    bool placed;
    bool specialSelected;
    GameObject canvas;
    LevelEditor levelEditor;

    // Start is called before the first frame update
    void Start()
    {
        levelEditor = GetComponent<LevelEditor>();
        placed = false;
        movePoint = transform.position;
        GameObject.FindObjectOfType<CanvasGroup>().interactable = false;
        
        if(EventSystem.current.currentSelectedGameObject.name == "btnSpecial")
        { specialSelected = true; }
        else { specialSelected = false; }
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Grid"));
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        //GameObject.Find("btnSpecial").GetComponent<Button>().enabled = false;
        //GameObject.Find("btnObjects").GetComponent<Button>().enabled = false;


        
        /* Andrew's move code - Should be in update, I think.
         * 
         * objectPosY++;
           Vector2 pos = new Vector2(objectPosX * 1f + 0.5f, objectPosY * 1f + 0.5f);
           transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime); */
    }

    // Update is called once per frame
    void Update()
    {

        if (placed == false)
        {
            if (Vector3.Distance(transform.position, movePoint) <= .05f)
            {
                /*
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                { movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f); }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                { movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f); }
                */

                if (Input.GetKeyDown(KeyCode.LeftArrow)){
                    if (objectPosX > 0){
                        movePoint += new Vector3(-1f, 0f, 0f);
                        objectPosX -= 2; }
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow)){
                    if (objectPosX < 12){
                        movePoint += new Vector3(1f, 0f, 0f);
                        objectPosX += 2; }
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow)){
                    if (objectPosY < 8){
                        movePoint += new Vector3(0f, 1f, 0f);
                        objectPosY += 2; }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)){
                    if (objectPosY > 0){
                        movePoint += new Vector3(0f, -1f, 0f);
                        objectPosY -= 2; }
                }
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject.FindObjectOfType<CanvasGroup>().interactable = true; 
                Destroy(gameObject);
            }


            else if (Input.GetKeyDown(KeyCode.Space))
            {
                //string placeName = gameObject.name;

                Debug.Log("Add: " + objectPosX + "," + objectPosY + ": " + gameObject.name);
                levelEditor.AddObject(objectPosX, objectPosY, gameObject.name);
                placed = true;
                GameObject.FindObjectOfType<CanvasGroup>().interactable = true;
                //GameObject.Find("btnSpecial").GetComponent<Button>().enabled = true;
                //GameObject.Find("btnObjects").GetComponent<Button>().enabled = true;
                if (specialSelected == true)
                { EventSystem.current.SetSelectedGameObject(GameObject.Find("btnSpecial")); }
                else
                { EventSystem.current.SetSelectedGameObject(GameObject.Find("btnObjects")); }
            }

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(objectPosX + "," + objectPosY);
            //string layoutName;
            levelEditor.CheckObject(objectPosX, objectPosY);
        }

        //Try to change layering as sprite is move backwards
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10 - objectPosY;

        transform.position = Vector3.MoveTowards(transform.position, movePoint, moveSpeed * Time.deltaTime);


        
       /* if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            objectPosY++;
            Vector2 pos = new Vector2(objectPosX * 1f + 0.5f, objectPosY * 1f + 0.5f);
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            //transform.position += Vector3(0,0,0);
        } */
        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public string moveObject;

    int playerPosX;
    int playerPosY;
    int newPosX;
    int newPosY;
    string[,] layout;

    
    void Start()
    {
        // Load the variable layout with the current map.
        // layout will be used to adjust the map and keep track of the objects.

        layout = LevelData.openLevel.layout;

        movePoint.parent = null;

        playerPosX = 0;
        playerPosY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
       // On axis movement, MovementCheck() is called to check for any obstacles it currently returns a bool
       // maybe it should return a string advising not only whether to move but also if another object needs to move.


        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
                 
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                int moveX = (int)Input.GetAxisRaw("Horizontal");

                bool canMove = MovementCheck("H" + moveX);
                Debug.Log("canMove " + canMove);

                if (canMove == true)
                {
                    movePoint.position += new Vector3(moveX, 0f, 0f);
                    //playerPosX += (int)Input.GetAxisRaw("Horizontal");
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                int moveY = (int)Input.GetAxisRaw("Vertical");

                bool canMove = MovementCheck("V" + moveY);
                Debug.Log("canMove " + canMove);

                if (canMove == true)
                {
                    movePoint.position += new Vector3(0f, moveY, 0f);
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder -= moveY*2;
                    //playerPosY += (int)Input.GetAxisRaw("Vertical");
                }


            }
            Debug.Log(moveObject);
            if (moveObject != "null")
            {
                Vector3 objectMovePoint = GameObject.Find(moveObject).transform.position + (movePoint.position - transform.position);
            }

        }

            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            
        /*
        if (newPosX != playerPosX && newPosY != playerPosY)
        {
            if (layout[newPosX, newPosY] == "rubbish_bin")
            { newPosX = playerPosX; newPosY = playerPosY; }
        }
        */


        //if (Vector3.Distance(transform.position, movePoint.position) <= .05f)

    }
    bool MovementCheck(string direction)
    {
        moveObject = "";
        Debug.Log(direction);
        Debug.Log("Player: " + playerPosX+","+playerPosY + " New: " + newPosX + "," +newPosY);

        switch (direction)
        {
            case "H1":
                newPosX = playerPosX + 2;
                newPosY = playerPosY;
                break;
            case "H-1":
                newPosX = playerPosX - 2;
                newPosY = playerPosY;
                break;
            case "V1":
                newPosX = playerPosX;
                newPosY = playerPosY + 2;
                break;
            case "V-1":
                newPosX = playerPosX;
                newPosY = playerPosY - 2;
                break;
            default:
                Debug.Log("Error in MovementCheck Switch");
                return true;
        }

        if (newPosX < 0) { newPosX = 0; }
        else if (newPosX >= layout.GetLength(0)-1) { newPosX = layout.GetLength(0)-1; }
        
        if (newPosY < 0) { newPosY = 0; }
        else if (newPosY >= layout.GetLength(1)-1) { newPosY = layout.GetLength(1)-1; }
       
        if (newPosX == playerPosX && newPosY == playerPosY) { return false; }
        
        if (newPosX > playerPosX)
        {
            Debug.Log((newPosX - 1) + "," + newPosY + "," + layout[newPosX - 1, newPosY]);
            if (layout[newPosX - 1, newPosY].Contains("barrier")) { return false; }
            if (layout[newPosX - 1, newPosY].Contains("detector"))
            {
                //Code to handle walking through the metal detector
                //Make this a function to call instead or put code outside of
                //these conditionals to save repeating it.
            }

        }
        else if (newPosX < playerPosX)
        {
            Debug.Log((newPosX + 1) + "," + newPosY + "," + layout[newPosX + 1, newPosY]);
            if (layout[newPosX + 1, newPosY].Contains("barrier")) { return false; }
            if (layout[newPosX + 1, newPosY].Contains("detector"))
            {
                //Code to handle walking through the metal detector
                //Make this a function to call instead or put code outside of
                //these conditionals to save repeating it.
            }
        }
        else if (newPosY > playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY - 1) + "," + layout[newPosX, newPosY - 1]);
            if (layout[newPosX, newPosY - 1].Contains("barrier")) { return false; }
            if (layout[newPosX, newPosY - 1].Contains("detector"))
            {
                //Code to handle walking through the metal detector
                //Make this a function to call instead or put code outside of
                //these conditionals to save repeating it.
            }
        }
        else if (newPosY < playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY + 1) + "," + layout[newPosX, newPosY + 1]);
            if (layout[newPosX, newPosY + 1].Contains("barrier")) { return false; }
            if (layout[newPosX, newPosY + 1].Contains("detector"))
            {
                //Code to handle walking through the metal detector
                //Make this a function to call instead or put code outside of
                //these conditionals to save repeating it.
            }
        }

        Debug.Log("HEY!  "+layout[newPosX, newPosY]);

        
        if (layout[newPosX, newPosY].Contains("rubbish")) { return false; }
        else if (layout[newPosX, newPosY].Contains("plant")) { return false; }
        else if (layout[newPosX, newPosY].Contains("luggage"))
        {
            //Code to move luggage object forward or possibly set a Bool
            //that triggers code inside player movement to move luggage object
            playerPosX = newPosX; playerPosY = newPosY;
            moveObject = layout[newPosX, newPosY];

            
            return true;
        }
        else if (layout[newPosX, newPosY].Contains("snack"))
        {
            //Code for the Snack Machine
            return false;
        }
        else if (layout[newPosX, newPosY].Contains("detector"))
        {
            // Code for emptying pockets into X-Ray machine
            // before walking through metal detector (see other "detector" code above)
            //    return false;
            return true;
        }
        else if (layout[newPosX, newPosY].Contains("mask"))
        {
            //Code for getting a mask
            return false;
        }
        else if (layout[newPosX, newPosY].Contains("gate"))
        {
            //Code for ending the level
            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }

        

        else { 
        playerPosX = newPosX; playerPosY = newPosY; return true;
       }
        
    }
}

//enum grid[10,10]

//vector2 pos = vector2 (x * 1 + 0.5,  y * 1 + 0.5);

// Below is some code that doesn't work but I want to find out why... later.
/*
 *   Gav's code that didn't work.. ignore
 * 
 * 
 *  Why doesn't this work???
 *  
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f || Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        { 
            moveH = (int)Input.GetAxisRaw("Horizontal");
            moveV = (int)Input.GetAxisRaw("Vertical");
            Debug.Log(moveH + "," + moveV);
        Debug.Log("Player: " + playerX + " NewX: " + newX);
        Debug.Log("H: " + Input.GetAxisRaw("Horizontal") + " V: " + Input.GetAxisRaw("Vertical"));
        newX = playerX + (int)Input.GetAxisRaw("Horizontal");
        Debug.Log("NewX2: " + newX);
        if (newX < 0) { newX = 0; }
        else if (newX == layout.GetLength(1)) { newX = layout.GetLength(1); }
        Debug.Log("NewX3: " + newX);

        newY = playerY + (int)Input.GetAxisRaw("Vertical");
        if (newY < 0) { newY = 0; }
        else if (newY >= layout.GetLength(0)) { newY = layout.GetLength(0); }
        Debug.Log(newX + "," + newY);
        /*if (layout[newX, newY] == "rubbish_bin")
        { newX = playerX; newY = playerY; }


        //if (newX != playerX || newY != playerY)
       
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
{



    if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)

    {
        //moveH = Input.GetAxisRaw("Horizontal");
        Debug.Log(Input.GetAxisRaw("Horizontal"));
        movePoint.position += new Vector3(moveH, 0f, 0f);//Input.GetAxisRaw("Horizontal"), 0f, 0f);
                                                         //playerX += (int)Input.GetAxisRaw("Horizontal");
                                                         //moveH = 0;
    }
    //else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
    else if (moveV != 0)
    {
        //moveV = Input.GetAxisRaw("Vertical");
        movePoint.position += new Vector3(0f, moveV, 0f);//0f, Input.GetAxisRaw("Vertical"), 0f);
                                                         //playerY += (int)Input.GetAxisRaw("Vertical");
                                                         //moveV = 0;
    }
    Debug.Log(moveH + "," + moveV);
}

transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            
        }*/
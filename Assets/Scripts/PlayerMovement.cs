using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    enum stuffInGird{
        nothing = 0,
        obstacle,
        box,
        snack,
        mask,
        gate,
        detector


    }

    public float moveSpeed = 5f;
    public Transform movePoint;

    int playerPosX;
    int playerPosY;
    int newPosX;
    int newPosY;
    Level levelScript;

    //stuffInGird[,] grid = new stuffInGird[10, 10];
    string[,] layout;

    // Start is called before the first frame update
    void Start()
    {
        levelScript = GameObject.Find("Main Camera").GetComponent<Level>();
        //layout = GameObject.Find("Main Camera").GetComponent<Level>().layout;
        layout = levelScript.GetLayout();
        Debug.Log("Got the array: " + layout[0, 2]);
        movePoint.parent = null;
        playerPosX = 0;
        playerPosY = 0;

        /* Need to add player sprite
        
        for (int x = 0; x < layout.GetLength(0); ++x)
        {
            for (int y = 0; y < layout.GetLength(1); ++y)
            { if (layout[x, y] == "player") { playerPosX = x; playerPosY = y; } }
        }*/
        
            /*
            if(grid[newX, newY]  == stuffInGird.nothing)
            {
                playersPositionY++;

                Vector2 pos = new Vector2(playersPositionX * 1f + 0.5f, playersPositionY * 1f + 0.5f);
                transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
            }
            else if(grid[newX, newY] == stuffInGird.obstacle)
            {
                //nothing
            }
            else if(grid[newX, newY] == stuffInGird.box)
            {
                //move box sprite to new player position + 1
                    //get name of prefab in position, transform prefab position + 1
                //move player position
            }
            else if (grid[newX, newY] == stuffInGird.snack)
            {
                //Snack = true
                //(Turns = Turns + X) or done at end of level
                //player doesn't move
            }
            else if (grid[newX, newY] == stuffInGird.mask)
            {
                //Mask = true
                //(Turns = Turns + X) or done at end of level
                //player doesn't move
            }
            else if (grid[newX, newY] == stuffInGird.detector)
            {
                //Mask = true
                //(Turns = Turns + X) or done at end of level
                //player doesn't move
            } 
            */

            // grid[0, 0] = stuffInGird.player;
    }

    // Update is called once per frame
    void Update()
    {
        
       
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                string direction = "H" + Input.GetAxisRaw("Horizontal");
                bool canMove = MovementCheck(direction);
                Debug.Log("canMove " + canMove);
                if (canMove == true)
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    //playerPosX += (int)Input.GetAxisRaw("Horizontal");
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                string direction = "V" + Input.GetAxisRaw("Vertical");
                bool canMove = MovementCheck(direction);
                Debug.Log("canMove " + canMove);
                if (canMove == true)
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    //playerPosY += (int)Input.GetAxisRaw("Vertical");
                }


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

        Debug.Log("Player2: " + playerPosX + "," + playerPosY + " New2: " + newPosX + "," + newPosY);
        if (newPosX < 0) { newPosX = 0; }
        else if (newPosX >= layout.GetLength(1)-1) { newPosX = layout.GetLength(1)-1; }
        Debug.Log("Player3: " + playerPosX + "," + playerPosY + " New3: " + newPosX + "," + newPosY);

        if (newPosY < 0) { newPosY = 0; }
        else if (newPosY >= layout.GetLength(0)-1) { newPosY = layout.GetLength(0)-1; }
        Debug.Log("Player4: " + playerPosX + "," + playerPosY + " New4: " + newPosX + "," + newPosY);

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
        //if (layout[newPosX, newPosY].Contains("rubbish")) { return false; }
        //else if (layout[newPosX, newPosY].Contains("plant")) { return false; }
        if (layout[newPosX, newPosY].Contains("luggage"))
        {
            //Code to move luggage object forward or possibly set a Bool
            //that triggers code inside player movement to move luggage object
            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }
        else if (layout[newPosX, newPosY].Contains("snack"))
        {
            //Code for the Snack Machine
            return false;
        }
        //else if (layout[newPosX, newPosY].Contains("detector"))
        //{
            // Code for emptying pockets into X-Ray machine
            // before walking through metal detector (see other "detector" code above)
        //    return false;
        //}
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



        else { playerPosX = newPosX; playerPosY = newPosY; return true; }
        
    }
}

//enum grid[10,10]

//vector2 pos = vector2 (x * 1 + 0.5,  y * 1 + 0.5);


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
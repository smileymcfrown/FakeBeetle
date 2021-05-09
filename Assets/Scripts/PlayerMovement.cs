using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Transform movePoint;
    public Vector3 objectMovePoint;
    public string moveObject = "nope";
    GameObject shuffle;  //Object the player is moving
    Level level;

    int playerPosX;
    int playerPosY;
    int newPosX;
    int newPosY;
    string[,] layout;
    bool endLevel = false;

    void Start()
    {
        // Load the variable layout with the current map.
        // layout will be used to adjust the map and keep track of the objects.
        level = FindObjectOfType<Level>();
        layout = LevelData.openLevel.layout;

        movePoint.parent = null;
        if (layout[12, 0] == "player")
        { playerPosX = 12; }
        else { playerPosX = 0; }
        playerPosY = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Level.paused)
        {

            // On axis movement, MovementCheck() is called to check for any obstacles it currently returns a bool
            // maybe it should return a string advising not only whether to move but also if another object needs to move.

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                Debug.Log("1");
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    int moveX = (int)Input.GetAxisRaw("Horizontal");
                    Debug.Log("2");

                    bool canMove = MovementCheck("H" + moveX);
                    Debug.Log("HcanMove " + canMove);

                    if (canMove == true)
                    {
                        movePoint.position += new Vector3(moveX, 0f, 0f);
                        if (moveObject != "nope")
                        { Debug.Log("INSIDE " + moveObject); shuffle.GetComponent<SpriteRenderer>().sortingOrder -= moveX; }
                        Level.turnsRemaining--;
                        level.UpdateHUD();

                        //Both of these have issues - says no Object Reference, but it's there... fucking unity.
                        //level.turnsText.text = string.Format("Turns: {0}", Level.turnsRemaining.ToString("D3"));
                        //level.Turns(); 

                        //playerPosX += (int)Input.GetAxisRaw("Horizontal");
                    }
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    int moveY = (int)Input.GetAxisRaw("Vertical");
                    Debug.Log("3");
                    bool canMove = MovementCheck("V" + moveY);
                    Debug.Log("VcanMove " + canMove);

                    if (canMove == true)
                    {
                        movePoint.position += new Vector3(0f, moveY, 0f);
                        gameObject.GetComponent<SpriteRenderer>().sortingOrder -= moveY * 26;
                        if (moveObject != "nope")
                        { Debug.Log("INSIDE " + moveObject); shuffle.GetComponent<SpriteRenderer>().sortingOrder -= moveY * 26; }
                        Level.turnsRemaining--;
                        level.UpdateHUD();
                        Debug.Log("3.1");
                        //playerPosY += (int)Input.GetAxisRaw("Vertical");
                    }


                }
                //Debug.Log("moveObject: " + moveObject);

            }
        

            if (endLevel && Vector3.Distance(transform.position, movePoint.position) <= .2f)
            {
                Debug.Log("LevelShouldComplete");
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;

                if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
                { 
                    Level.paused = true; 
                    Level.timeTaken = Time.time - Level.startTime;
                    Debug.Log("LevelComplete " + "Time: " + Level.timeTaken + " Paused: " + Level.paused);
                    level.LevelComplete(); 
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
            if (moveObject != "nope")
            {
                objectMovePoint = shuffle.transform.position + (movePoint.position - transform.position);
                shuffle.transform.position = Vector3.MoveTowards(shuffle.transform.position, objectMovePoint, moveSpeed * Time.deltaTime);
            }

            if(Level.turnsRemaining == 0)
            {
                Level.paused = true;
                level.Death();
            }

        }

        //if (Vector3.Distance(transform.position, movePoint.position) <= .05f)

    }
    bool MovementCheck(string direction)
    {
        //Debug.Log(direction);
        //Debug.Log("Player: " + playerPosX + "," + playerPosY + " New: " + newPosX + "," + newPosY);

        moveObject = "nope";

        int barrierX;
        int barrierY;
        int secondObjectX;
        int secondObjectY;

        switch (direction)
        {
            case "H1":
                newPosX = playerPosX + 2;
                newPosY = playerPosY;
                barrierX = playerPosX + 3;
                barrierY = playerPosY;
                secondObjectX = playerPosX + 4;
                secondObjectY = playerPosY;
                break;
            case "H-1":
                newPosX = playerPosX - 2;
                newPosY = playerPosY;
                barrierX = playerPosX - 3;
                barrierY = playerPosY;
                secondObjectX = playerPosX - 4;
                secondObjectY = playerPosY;
                break;
            case "V1":
                newPosX = playerPosX;
                newPosY = playerPosY + 2;
                barrierX = playerPosX;
                barrierY = playerPosY + 3;
                secondObjectX = playerPosX;
                secondObjectY = playerPosY + 4;
                break;
            case "V-1":
                newPosX = playerPosX;
                newPosY = playerPosY - 2;
                barrierX = playerPosX;
                barrierY = playerPosY - 3;
                secondObjectX = playerPosX;
                secondObjectY = playerPosY - 4;
                break;
            default:
                Debug.Log("Error in MovementCheck Switch");
                return true;
        }
        
        // ***************** Boarding Gate - Level End *****************

        if( layout[playerPosX, playerPosY].Contains("mask") && direction == "V1") { return false; }
        else if (layout[playerPosX, playerPosY].Contains("gate"))
        {
            if (layout[12, 8].Contains("gate"))
            {
                Debug.Log("4");

                if (newPosX > layout.GetLength(0) - 1)
                { newPosX = layout.GetLength(0) - 1; }
                
            }
            else
            {
                Debug.Log("5");

                if (newPosX < 0)
                { newPosX = 0; }
            }
            if (direction == "V1")
            {
                if (Level.gotMask == true)
                {
                    Debug.Log("6");

                    movePoint.position += new Vector3(0f, 1, 0f);
                    transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
                    Level.turnsRemaining--;
                    level.UpdateHUD();
                    endLevel = true;
                }
                Debug.Log("7");
                return false;
            }
        }

        // ***************** Keep player inside game area *****************

        else if (layout[12, 0] == "player")
        {
            if (newPosX < 0) { newPosX = 0; }
            else if (newPosX >= layout.GetLength(0) - 1) { newPosX = layout.GetLength(0) - 1; }

            if (newPosY < 0) { newPosY = 0; }
            else if (newPosY >= layout.GetLength(1) - 1) { newPosY = layout.GetLength(1) - 1; }
        }
        else
        {
            if (newPosX < 0) { newPosX = 0; }
            else if (newPosX >= layout.GetLength(0) - 1) { newPosX = layout.GetLength(0) - 1; }

            if (newPosY < 0) { newPosY = 0; }
            else if (newPosY >= layout.GetLength(1) - 1) { newPosY = layout.GetLength(1) - 1; }
        }

        if (newPosX == playerPosX && newPosY == playerPosY) { return false; }

        // ***************** Check for barrier objects *****************

        if (newPosX > playerPosX)
        {
            Debug.Log((newPosX - 1) + "," + newPosY + "," + layout[newPosX - 1, newPosY]);
            if (layout[newPosX - 1, newPosY].Contains("barrier")) { return false; }
        }
        else if (newPosX < playerPosX)
        {
            Debug.Log((newPosX + 1) + "," + newPosY + "," + layout[newPosX + 1, newPosY]);
            if (layout[newPosX + 1, newPosY].Contains("barrier")) { return false; }
        }
        else if (newPosY > playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY - 1) + "," + layout[newPosX, newPosY - 1]);
            if (layout[newPosX, newPosY - 1].Contains("barrier")) { return false; }
            if (layout[newPosX, newPosY - 1].Contains("detector"))
            {
                Debug.Log("You're going through the detector!");
            }
        }
        else if (newPosY < playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY + 1) + "," + layout[newPosX, newPosY + 1]);
            if (layout[newPosX, newPosY + 1].Contains("barrier")) { return false; }
            if (layout[newPosX, newPosY + 1].Contains("detector")) { return false; }
        }

        // ***************** Check for, and move, luggage *****************

        if (layout[newPosX, newPosY].Contains("luggage"))
        {
            //Code to move luggage object forward or possibly set a Bool
            //that triggers code inside player movement to move luggage object
            
            if (secondObjectX < 0 || secondObjectX >= layout.GetLength(0) ||
                secondObjectY < 0 || secondObjectY >= layout.GetLength(1) || 
                layout[barrierX, barrierY] != "empty" || layout[secondObjectX, secondObjectY] != "empty")
                // || !layout[secondObjectX, secondObjectY].Contains("mask") )
            {
                return false;
            }
            else
            {
                //if(layout[secondObjectX, secondObjectY].Contains("mask"))
                //{
                    //Put some code here to keep mask in position while luggage occupies the same space
                    //Possibly give Mask a position marker at start using y * (13 + x) to convert position
                    //instead of relying on layout[] which will be changed when luggage takes mask's tile
               // }
                //Debug.Log("CHECK " + moveObject);
                moveObject = layout[newPosX, newPosY];
                layout[secondObjectX, secondObjectY] = moveObject;
                shuffle = GameObject.Find(moveObject);

                layout[playerPosX, playerPosY] = "empty";
                playerPosX = newPosX; playerPosY = newPosY;
                return true;
            }
        }

        // ***************** Check for other objects *****************

        else if (layout[newPosX, newPosY].Contains("snack"))
        {
            if (!Level.gotSnack)
            {
                Level.gotSnack = true;
                Level.turnsRemaining += 5;
            }
            return false;
        }
        else if (layout[newPosX, newPosY].Contains("detector"))
        {
            // Code for emptying pockets into X-Ray machine
            // before walking through metal detector (see other "detector" code above)
            // If coming from above: return false
            //    return false;
            Debug.Log("Detector Pockets!");
            layout[playerPosX, playerPosY] = "empty";
            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }
        else if (layout[newPosX, newPosY].Contains("x-ray"))
        {
            Debug.Log("X-Ray Pockets!");
            // Code for emptying pockets into X-Ray machine
            // before walking through metal detector (see other "detector" code above)
            // if !coming from right: return false
            return false;
        }
        else if (layout[newPosX, newPosY].Contains("mask"))
        {
            if(direction == "V-1") { return false; }

            Level.gotMask = true;

            layout[playerPosX, playerPosY] = "empty";
            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }
        else if (layout[newPosX, newPosY].Contains("rubbish")) { return false; }
        else if (layout[newPosX, newPosY].Contains("plant")) { return false; }


        /* The code below is for ending the level when the player reaches the *last square*.
         * 
         * Currently the 'gate' code is on line 156 which means the player must reach the last square
         * and then press 'up' to enter the gate (and requiring one more turn).
         * 
        else if (layout[newPosX, newPosY].Contains("gate"))
        {
            // Code here for ending the level.

            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }
        */


        else {
        layout[playerPosX, playerPosY] = "empty";
        playerPosX = newPosX; playerPosY = newPosY;
        return true;
       }
        
    }
}

/*
 * enum grid[10,10];
 * vector2 pos = vector2 (x * 1 + 0.5,  y * 1 + 0.5);
*/

 //Below is some code that doesn't work but I want to find out why... later.

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
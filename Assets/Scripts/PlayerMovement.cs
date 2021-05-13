using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 3f;
    public Transform movePoint;
    public Sprite greenLight; // Store sprites to change the Metal Detector light. Not the most appropriate
    public Sprite redLight;   // place for them, but what the hell, all the Metal Detector code is in here! 
    
    GameObject shuffle;  // Object the player is moving...
    Vector3 objectMovePoint; // it's position...
    string moveObject = "none"; // and it's name - "none" if there is no object to move.

    // Level layout and positioning
    Level level;
    public static string[,] tempLayout;
    int playerPosX; 
    int playerPosY;
    int newPosX; 
    int newPosY;

    // Milestones
    bool gotMask = false;
    bool xray = false;
    bool detectorPass = false;
    bool gotSnack = false;
    bool endLevel = false;

    void Start()
    {
        // Get Level script to access variables
        level = FindObjectOfType<Level>();

        // tempLayout[] will be used to adjust the map and keep track of the objects.
        tempLayout = LevelData.openLevel.layout;

        // Stop movePoint being a child of Player object
        movePoint.parent = null;

        //Set up Player starting position - adjust X position depending on level design
        if (tempLayout[12, 0] == "player") { playerPosX = 12; }
        else { playerPosX = 0; }
        playerPosY = 0;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Level.paused = true;  level.Death(); } // Lazy way to quit without a confirmation menu or more code.

        // Pausing movement whilst allowing coroutines to run for menu transitions.
        if(!Level.paused)
        {
            // ***** Input - On axis movement, true direction found, and MovementCheck() called to check obstacles.
            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                // Death - Wait until player has settled on their new square before killing them.
                // Check for no more turns and end the game.
                if (Level.turnsRemaining == 0)
                {
                    Level.paused = true;
                    level.Death();
                }

                // Check for Horizontal Axis
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    int moveX = (int)Input.GetAxisRaw("Horizontal"); // Get axis direction, 1 or -1.
                    bool canMove = MovementCheck("H" + moveX); // Call to check for obstacles with full direction, "H1" or "H-1".

                    if (canMove == true)
                    {
                        movePoint.position += new Vector3(moveX, 0f, 0f);

                        gameObject.GetComponent<SpriteRenderer>().sortingOrder -= moveX * 2;
                        if (moveObject != "none") { shuffle.GetComponent<SpriteRenderer>().sortingOrder -= moveX * 2; }

                        Level.turnsRemaining--;
                        level.UpdateHUD();

                        //Both of these have issues - says no Object Reference, but it's there... stupid unity.
                        //level.turnsText.text = string.Format("Turns: {0}", Level.turnsRemaining.ToString("D3"));
                        //level.Turns(); 
                    }
                }

                // Check for Vertical Axis
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    int moveY = (int)Input.GetAxisRaw("Vertical");
                    bool canMove = MovementCheck("V" + moveY); // Call to check for obstacles with full direction, "V1" or "V-1".

                    if (canMove == true)
                    {
                        movePoint.position += new Vector3(0f, moveY, 0f);

                        // Change sprite sorting order for player (and luggage)
                        // unless player is going through metal detector (see below).
                        if (!detectorPass) { gameObject.GetComponent<SpriteRenderer>().sortingOrder -= moveY * 26; }
                        if (moveObject != "none") { shuffle.GetComponent<SpriteRenderer>().sortingOrder -= moveY * 26; }
                        
                        Level.turnsRemaining--;
                        level.UpdateHUD();
                    }
                }
            }

            // Move player if needed.
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            //  Player sprite sorting order - Delay change so Player doesn't appear to walk through the base of Metal Detector.
            if (detectorPass && Vector3.Distance(transform.position, movePoint.position) <= .1f)
            {
                gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 26;
                detectorPass = false;
            }

            // Luggage - Check to see if it needs to be moved and move it.
            if (moveObject != "none")
            {
                objectMovePoint = shuffle.transform.position + (movePoint.position - transform.position);
                shuffle.transform.position = Vector3.MoveTowards(shuffle.transform.position, objectMovePoint, moveSpeed * Time.deltaTime);
            }

            // End Level - Set level time, and make Player go behind Boarding Gate at the right time.
            if (endLevel && Vector3.Distance(transform.position, movePoint.position) <= .2f)
            {
                Debug.Log("LevelShouldComplete");
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;

                if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
                {
                    Level.paused = true;
                    Level.timeTaken = Time.time - Level.levelStartTime;
                    Level.totalTime = Time.time - Level.gameStartTime;
                    Debug.Log("LevelComplete " + "Time: " + Level.timeTaken + " Paused: " + Level.paused);
                    level.LevelComplete();
                }
            }
        }
    }

    // Checks for all obstacles and grid bounds, updates tempLayout[] with new positions, and returns TRUE if player can move.
    bool MovementCheck(string direction)
    {
        //Used to check which Luggage to move - Set to default "none".
        moveObject = "none";

        //Used to check for Barriers between grid positions and objects behind other objects (Second Object).
        int barrierX;
        int barrierY;
        int secondObjectX;
        int secondObjectY;

        // Check which direction (H1, V1, H-1, V-1) player is going
        // and load next positions for Player, Barrier, and Second Objects.
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
        
        
        // Mask - Standing on the Mask Dispenser Square? Can't move up!
        if( tempLayout[playerPosX, playerPosY].Contains("mask") && direction == "V1") { return false; }
        

        // ***** Boarding Gate / Level End
        // Standing on the square at the Gate?
        // Check which side it's on, check for mask, allow player to move outside the grid and finish.
        else if (tempLayout[playerPosX, playerPosY].Contains("gate"))
        {
            if (tempLayout[12, 8].Contains("gate"))
            {
                if (newPosX > tempLayout.GetLength(0) - 1)
                { newPosX = tempLayout.GetLength(0) - 1; }
                
            }
            else
            {
                if (newPosX < 0)
                { newPosX = 0; }
            }
            if (direction == "V1")
            {
                if (gotMask == true)
                {
                    movePoint.position += new Vector3(0f, 1, 0f);
                    transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
                    // Felt bad about making the player lose a turn taking the final step into the gate.
                    // Level.turnsRemaining--;
                    level.UpdateHUD();
                    endLevel = true;
                }
                return false;
            }
        }


        // ***** Grid Bounds - Keep player inside game area.
        else if (tempLayout[12, 0] == "player")
        {
            if (newPosX < 0) { newPosX = 0; }
            else if (newPosX >= tempLayout.GetLength(0) - 1) { newPosX = tempLayout.GetLength(0) - 1; }

            if (newPosY < 0) { newPosY = 0; }
            else if (newPosY >= tempLayout.GetLength(1) - 1) { newPosY = tempLayout.GetLength(1) - 1; }
        }
        else
        {
            if (newPosX < 0) { newPosX = 0; }
            else if (newPosX >= tempLayout.GetLength(0) - 1) { newPosX = tempLayout.GetLength(0) - 1; }

            if (newPosY < 0) { newPosY = 0; }
            else if (newPosY >= tempLayout.GetLength(1) - 1) { newPosY = tempLayout.GetLength(1) - 1; }
        }
        if (newPosX == playerPosX && newPosY == playerPosY) { return false; } 


        // ***** Barriers and Metal Detector *****
        // Up - check for the Metal Detector and stop player if necessary.
        if (newPosY > playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY - 1) + "," + tempLayout[newPosX, newPosY - 1]);
            if (tempLayout[newPosX, newPosY - 1].Contains("barrier")) { return false; }
            if (tempLayout[newPosX, newPosY - 1].Contains("detector"))
            {
                if (!xray)  // Add more to this.. move character forward change sprites move character back.
                {
                    Level.paused = true;
                    StartCoroutine(SecurityFail());
                    return false;
                }
                else { detectorPass = true; 
                    Debug.Log("You're going through the detector!"); }
            }
        }
        // Down
        else if (newPosY < playerPosY)
        {
            Debug.Log(newPosX + "," + (newPosY + 1) + "," + tempLayout[newPosX, newPosY + 1]);
            if (tempLayout[newPosX, newPosY + 1].Contains("barrier")) { return false; }
            if (tempLayout[newPosX, newPosY + 1].Contains("detector")) { return false; }
        }
        // Right - No vertical barrier sprites made for Right / Left... yet.
        else if (newPosX > playerPosX)
        {
            Debug.Log((newPosX - 1) + "," + newPosY + "," + tempLayout[newPosX - 1, newPosY]);
            if (tempLayout[newPosX - 1, newPosY].Contains("barrier")) { return false; }
        }
        // Left
        else if (newPosX < playerPosX)
        {
            Debug.Log((newPosX + 1) + "," + newPosY + "," + tempLayout[newPosX + 1, newPosY]);
            if (tempLayout[newPosX + 1, newPosY].Contains("barrier")) { return false; }
        }
        

        // ***** Luggage - Moving it around
        if (tempLayout[newPosX, newPosY].Contains("luggage"))
        {

            // Check for an object or grid edge *behind* the object you are trying to move.
            if (secondObjectX < 0 || secondObjectX >= tempLayout.GetLength(0) ||
                secondObjectY < 0 || secondObjectY >= tempLayout.GetLength(1) || 
                tempLayout[barrierX, barrierY] != "empty" || tempLayout[secondObjectX, secondObjectY] != "empty")
            { return false; }
            else
            {
                /* Code to allow Luggage to occupy same space as Mask - to difficult to implement in the timeframe.
                 * 
                if(layout[secondObjectX, secondObjectY].Contains("mask"))
                {
                    Put some code here to keep mask in position while luggage occupies the same space
                    Possibly give Mask a position marker at start using y * (13 + x) to convert position
                    instead of relying on layout[] which will be changed when luggage takes mask's tile
                }
               */

                // Set up the luggage in a gameObject ready to move in Update() ...
                moveObject = tempLayout[newPosX, newPosY];
                shuffle = GameObject.Find(moveObject);
                //  and update tempLayout and playerPos with new positions.
                tempLayout[secondObjectX, secondObjectY] = moveObject;
                tempLayout[playerPosX, playerPosY] = "empty";
                playerPosX = newPosX; playerPosY = newPosY;
                return true;
            }
        }

        // ***** Snacks - Add 5 Turns
        else if (tempLayout[newPosX, newPosY].Contains("snack"))
        {
            if (!gotSnack)
            {
                gotSnack = true;
                Level.turnsRemaining += 5;
                level.UpdateHUD();
            }
            return false;
        }
        
        // ***** X-Ray - Play Animation, Make player wait, Take away 1 turn.
        else if (tempLayout[newPosX, newPosY].Contains("x-ray"))
        {
            Debug.Log("X-Ray Pockets!");
            GameObject.Find("detector_v").GetComponent<Animator>().enabled = true;
            xray = true;
            Debug.Log("Xray " + gameObject.name + ": " + transform.position);
            StartCoroutine(Pockets());
            // Code for emptying pockets into X-Ray machine
            // before walking through metal detector (see other "detector" code above)
            // if !coming from right: return false
            Level.paused = true;
            Level.turnsRemaining--;
            Debug.Log("Xray Turns: " + Level.turnsRemaining);

            level.UpdateHUD();
            return false;
        }
        
        // ***** Mask - Set true to allow player to enter the Boarding Gate
        else if (tempLayout[newPosX, newPosY].Contains("mask"))
        {
            if(direction == "V-1") { return false; }

            gotMask = true;

            tempLayout[playerPosX, playerPosY] = "empty";
            playerPosX = newPosX; playerPosY = newPosY;
            return true;
        }
        
        // ***** Plants and Bins are immoveable
        else if (tempLayout[newPosX, newPosY].Contains("rubbish")) { return false; }
        else if (tempLayout[newPosX, newPosY].Contains("plant")) { return false; }

        // ***** Empty Square - Set new position and updates tempLayout[] so Player's previous position is empty
        else {
        tempLayout[playerPosX, playerPosY] = "empty";
        playerPosX = newPosX; playerPosY = newPosY;
        return true;
       }
    }


    // Delay and pause game while Player empties pockets into X-Ray tray.
    IEnumerator Pockets() {
        Debug.Log("9.5 second start" + Time.time);
        yield return new WaitForSeconds(9.6f);
        Debug.Log("finished: " + Time.time);
        Level.paused = false;

    }
    

    // Make Player attempt to pass through Metal Detector and fail whilst losing two turns
    IEnumerator SecurityFail()
    {
        bool goBack = false;
        bool up = true;
        bool down = true;
        Vector3 startPos = transform.position;
        Vector3 stopPos = transform.position; // transform.position + new Vector3(0,1f,0);
        stopPos.y++;   // Couldn't get above to work without creating and increasing separately

        /* Hacked together, messy code to make Player go forward, wait, and then go back whilst
         * adjusting sprite sorting order and changing detector lights to red and back to green
         * 
         * Probably could be done in a cleaner way (attempted to in rem'd code below this CoRoutine);
         * however, I don't know enough about CoRoutines and how they work to make it cleaner.
         */

        // Go up through Metal Detector
        while (transform.position.y <= stopPos.y && goBack == false)
        {
            // Change Player sprite sorting order at correct time for smooth transition.
            // Change Metal Detector light from GREEN to RED.
            if (transform.position.y >= stopPos.y - 0.1) // && goBack == false)
            {
                
                if (down) { gameObject.GetComponent<SpriteRenderer>().sortingOrder -= 26; down = false; }
                GameObject.Find("detector_v").GetComponent<SpriteRenderer>().sprite = redLight;
            }
            
            // Player stop, loses a turn, and pauses on other side of Metal Detector when next square reached.
            if (transform.position.y >= stopPos.y ) // && goBack == false)
            {
                Level.turnsRemaining--;
                level.UpdateHUD();
                goBack = true;
                yield return new WaitForSeconds(1);
            }

            transform.position = Vector3.MoveTowards(transform.position,stopPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Go back down through Metal Detector
        while (startPos.y <= transform.position.y && goBack == true)
        {
            // Change Player sprite sorting order back to original order and correct time.
            if (transform.position.y <= startPos.y + .9) // && goBack == false)
            {
                if (up) { gameObject.GetComponent<SpriteRenderer>().sortingOrder += 26; up = false; }
            }
            
            // Change Metal Detector light back from RED to GREEN
            if (transform.position.y <= startPos.y + .1) // && goBack == false)
            {
                GameObject.Find("detector_v").GetComponent<SpriteRenderer>().sprite = greenLight;
            }
            
            // Player back to original position - Lose another turn, unpause game.
            if (transform.position.y <= startPos.y ) // && goBack == true)
            {
                Debug.Log("Security Fail 4");
                Level.turnsRemaining--;
                level.UpdateHUD();
                Level.paused = false;
                yield break;
            }
            Debug.Log("Security Fail 3");

            //Why didn't this work intead? -> transform.position -= new Vector3(0, moveAmount * Time.deltaTime, 0); or += new Vector3(0,-(moveAmount * ...)
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            
            yield return null;
        }

        // Some CoRoutine examples had ending code down here, but when I did, it either failed or 
        // deducted many turns from the player.. maybe I had "yield return null;" missing or in the wrong place?
        //
        //Level.turnsRemaining -= 2;
        //Level.paused = false;
        //Debug.Log("Not Paused!");
        
        
    }


    // Two attempts at SecurityFail() that ironically, did indeed fail.
    /*
    IEnumerator SecurityFail()
    {
        float time = 0;
        //float duration = 4;
        Vector3 startPos = transform.position;
        Vector3 stopPos = transform.position + new Vector3(0f, 1, 0f);
        while (time <= 4f)
        {
            if(time < 1.5f)//(duration / 2) - 0.5f)
            {
                transform.position = Vector3.Lerp(startPos, stopPos, (time / 1.5f)); //((duration / 2) - 0.5f)));
                time += Time.deltaTime;
                yield return null;

            }
            //else if (time >= 1.5f && time < 2.5f)//(duration / 2) - 0.5f && time <= (duration / 2) + 0.5f)
            //{
            //   time += Time.deltaTime;
            //    yield return null;
            //}
            else if (time > 2.5f)// (duration / 2) + 0.5f && time < duration)
            {
                transform.position = Vector3.Lerp(stopPos, startPos, (time / 4f)); //((duration / 2) - 0.5f)));
                time += Time.deltaTime;
                yield return null;
            }
            else { time += Time.deltaTime; yield return null;}
        }

            //transform.position = startPos;
            Level.paused = false;
        
    }
   */

    /*
    private void SecurityFail()
    {
        Vector3 pingPos = transform.position;
        Vector3 pongPos = transform.position + new Vector3(0f, 1, 0f);

        float waitTime = 500f;
        float moveSpeed = 1000f;
        float timer = Mathf.PingPong(moveSpeed * Time.time, 1 + waitTime) - waitTime * 0.5f;
        timer = Mathf.Clamp(timer, 0, 30);
        transform.position = Vector3.Lerp(pingPos, pongPos, timer);
    } */
}
    //Below is some more code from the Input code in Update() that didn't work but I want to find out why... later.
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

                    movePoint.position += new Vector3(moveH, 0f, 0f); //Input.GetAxisRaw("Horizontal"), 0f, 0f);
                
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
        }
    */
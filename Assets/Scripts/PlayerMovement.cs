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


   public LayerMask whatStopsMovement;

   // If the code does not work, it is possible that the code below needs to be uncommented! 
   // This code was commented out to resolve a merge conflict between Developer and Main.
   // It's not my code, but no one else is resolving the merge conflicts in these pull requests.
   // Main had the code below and not the line abouve; Developer the opposite.
   //
   // int playersPositionX;
   // int playersPositionY;
   // int newPosX;
   // int newPosY;

   // stuffInGird[,] grid = new stuffInGird[10, 10];


    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;

        if(grid[newPosX, newPosY]  == stuffInGird.nothing)
        {
            playersPositionY++;

            Vector2 pos = new Vector2(playersPositionX * 1f + 0.5f, playersPositionY * 1f + 0.5f);
            transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed * Time.deltaTime);
        }
        else if(grid[newPosX, newPosY] == stuffInGird.obstacle)
        {
            //nothing
        }
        else if(grid[newPosX, newPosY] == stuffInGird.box)
        {
            //move box sprite to new player position + 1
                //get name of prefab in position, transform prefab position + 1
            //move player position
        }
        else if (grid[newPosX, newPosY] == stuffInGird.snack)
        {
            //Snack = true
            //(Turns = Turns + X) or done at end of level
            //player doesn't move
        }
        else if (grid[newPosX, newPosY] == stuffInGird.mask)
        {
            //Mask = true
            //(Turns = Turns + X) or done at end of level
            //player doesn't move
        }
      /*  else if (grid[newPosX, newPosY] == stuffInGird.detector)
        {
            //Mask = true
            //(Turns = Turns + X) or done at end of level
            //player doesn't move
        } */


        // grid[0, 0] = stuffInGird.player;




    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }

                // Code removed in merge conflict with if statement above.
                // movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);

            }
        }

         transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
    }
}

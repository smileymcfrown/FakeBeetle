using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour
{
    BoxCollider2D col2d;

    // Start is called before the first frame update
    void Start()
    {
        col2d = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
      float x = transform.position.x - col2d.size.x / 2;
       // if (Input.GetKeyDown(KeyCode.Space))
        {
            
            
            
            
            
            //transform.Translate(Vector2.right * col2d.size.x);

           // transform.position = new Vector2(transform.position.x + col2d.size.x, transform.position.y);
        }
    }

    // OnCollisionEnter2D is called when this collider2D/rigidbody2D has begun touching another rigidbody2D/collider2D (2D physics only)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if collision.collider.tag == other object... other object += direction

        if(collision.collider.tag == "Box")
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position = new Vector2(transform.position.x + col2d.size.x, transform.position.y);
            }
        }
    }


}

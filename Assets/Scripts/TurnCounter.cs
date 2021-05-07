using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    [SerializeField] public int turns;

    // Start is called before the first frame update
    void Start()
    {
        turns = 40;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            turns -= 1;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            turns -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            turns -= 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            turns -= 1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private TurnCounter turnsCounter;
    [SerializeField] public bool hasSnacks = false;
    [SerializeField] public int snackTurnAmount = 3;

    [SerializeField] public bool hasMask = false;
    [SerializeField] private GameObject gate;

    // Start is called before the first frame update
    void Start()
    {
        gate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMask)
        {
            gate.SetActive(true);
        }
        else
        {
            gate.SetActive(false);  
        }
    }
   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "SnackMachine")
        {
            SnackMachine();
        }
        if (collision.collider.tag == "MaskStation")
        {
            MaskStation();
        }

    }
    public void SnackMachine()
    {
        if (hasSnacks == false)
        {
            Debug.Log("Got Snacks");
            turnsCounter.turns += snackTurnAmount;
            hasSnacks = true;
        }
    }
    public void MaskStation()
    {
        if (hasMask == false)
        {
            Debug.Log("Got Mask");
            gate.SetActive(false);
            hasMask = true;
        }
    }
}

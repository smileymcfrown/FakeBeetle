using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private TurnCounter turnsCounter;
    [SerializeField] private bool hasSnacks = false;
    [SerializeField] private int snackTurnAmount = 3;

    [SerializeField] private bool hasMask = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SnackMachine"))
        {
            Debug.Log("Got Snacks");
            SnackMachine();
        }
        if (collision.gameObject.CompareTag("MaskStation"))
        {
            Debug.Log("Got Mask");
            MaskStation();
        }

    }
    public void SnackMachine()
    {
        if (hasSnacks == false)
        {
            turnsCounter.turns += snackTurnAmount;
            hasSnacks = true;
        }
    }
    public void MaskStation()
    {
        if (hasMask == false)
        {
            gate.SetActive(false);
            hasMask = true;
        }
    }
}

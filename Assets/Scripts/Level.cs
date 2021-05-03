using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    Vector3 startPos;
    public GameObject[,] objects = new GameObject[7, 9];

    // Start is called before the first frame update
    void Start()
    {
        
        /*
        if (File.Exists(FileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            PaR newData1 = (PaR)bf.Deserialize(file);
            for (int i = 0; i < Object.Length; i++)
            {
                Object[i].transform.position = new Vector3(newData1.OPX, newData1.OPY, newData1.OPZ);
            }
            file.Close();
        }
        */

        startPos = new Vector3(-2.7f, -3.25f, 0f);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    public GameObject objectsPrefab;
    public Sprite[] spriteList;
    Vector3 startPos;
    Vector3 currentPos;
    Sprite objectSprite;
    
    string[,] layout = new string[13, 9]    
    {
        { "empty",      "empty" , "rubbish_bin" ,   "empty" , "luggage_02" ,    "empty" ,       "rubbish_bin" ,   "empty" , "empty" } ,
        { "empty",      "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" },
        { "empty",      "empty" , "snack_machine" , "empty" , "empty" ,         "empty" ,       "luggage_06" ,    "empty" , "mask_dispenser" },
        { "empty",      "empty" , "barrier_left" ,  "empty" , "barrier_single" ,"empty" ,       "barrier_right" , "empty" , "empty" },
        { "empty",      "empty" , "luggage_04" ,    "empty" , "empty" ,         "empty" ,       "luggage_01" ,    "empty" , "empty" },
        { "empty",      "empty" , "empty" ,         "empty" , "empty" ,         "metal_detector" ,"empty" ,       "empty" , "empty"  },
        { "luggage_01", "empty" , "barrier_left" ,  "empty" , "plant_02" ,      "empty" ,       "empty",          "empty" , "luggage_03" },
        { "empty",      "empty" , "barrier_single" ,"empty" , "empty" ,         "metal_detector" ,"empty" ,       "empty" , "empty" },
        { "luggage_01", "empty" , "barrier_right" , "empty" , "plant_02" ,      "empty" ,       "empty" ,         "empty" , "luggage_03"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" } ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "plant_01"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty",        "empty" ,         "empty" , "empty"} ,
        { "empty" ,     "empty" , "empty" ,         "empty" , "empty" ,         "empty" ,       "empty" ,         "empty" , "empty" }};
    // public GameObject[,] objects = new GameObject[7, 9];

    // Start is called before the first frame update

 
void Start()
    {
        startPos = new Vector3(-2.7f, -3.25f, 0f);
        // Get layout Array
        for (int x = 0; x < layout.GetLength(0); ++x)
        { for (int y = 0; y < layout.GetLength(1); ++y)
            {
                if (layout[x, y] != "empty" )
                {
                    for (int i = 0; i < spriteList.Length; ++i)
                    {
                        if (spriteList[i].name == layout[x, y])
                        {
                            objectSprite = spriteList[i];
                        }

                    }
                    currentPos = startPos + new Vector3(x/2, y/2, 0);
                    GameObject newObject = Instantiate(objectsPrefab, currentPos, Quaternion.identity);
                    newObject.name = objectSprite.name; //= objectName;
                    newObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
                }
            }
        }
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



    }


    public string[,] GetLayout()
    {
        return layout;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

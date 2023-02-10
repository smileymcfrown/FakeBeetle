using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    public GameObject contentPanel;
    public GameObject listItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (LevelData level in LoadSave.savedLevels)
        {
            GameObject newItem = Instantiate(listItemPrefab,contentPanel.transform) as GameObject;
            newItem.GetComponent<Text>().text = level.levelName;
            //newItem.transform.parent = contentPanel.transform;
            newItem.transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

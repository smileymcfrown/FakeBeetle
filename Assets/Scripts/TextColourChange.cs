using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextColourChange : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Color normal;
    public Color select;
    
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = select;

    }

    public void OnDeselect(BaseEventData eventData)
    {
        gameObject.GetComponentInChildren<Text>().color = normal;

    }
}

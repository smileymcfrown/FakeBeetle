using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FullScreenMode : MonoBehaviour
{
    private Toggle toggle;
      

   void Start()
   {
        toggle = gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(_fullscreen => Screen.fullScreen = _fullscreen);


   }
    
}

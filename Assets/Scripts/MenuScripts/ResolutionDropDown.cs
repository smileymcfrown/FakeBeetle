using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ResolutionDropDown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private  List <Resolution> resolutions;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        // Get all of the resolutions supported by this monitor
        resolutions = new List<Resolution> (Screen.resolutions);
        dropdown.onValueChanged.AddListener(OnResolutionChanged);
        SetupOptions();
    }


    private void SetupOptions()
    {
        // Remove all currently created options on the dropdown.
        dropdown.ClearOptions();


        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();

        // Loop through all resolution settings
        foreach (Resolution resolution in resolutions)
        {
            // create a new option on the name and add it to the dropdown
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(resolution.ToString());
            newOptions.Add(option);
        }

        // Add all the new options to the dropdown
        dropdown.AddOptions(newOptions);
        //Set the current value to the current quality level and show the correct value
        dropdown.value = resolutions.IndexOf(Screen.currentResolution);
        dropdown.RefreshShownValue();
    }

    private void OnResolutionChanged(int _resolution)
    {
        Resolution resolution = resolutions[_resolution];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
}

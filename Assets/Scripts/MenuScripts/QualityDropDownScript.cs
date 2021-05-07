using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]

public class QualityDropDownScript : MonoBehaviour
{

    private TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        // Setup the options based on the quality settings
        SetupOptions();

        // Make out OnOptionsChanged function listen for the value to change on the
        // dropdown.
        dropdown.onValueChanged.AddListener(OnOptionChanged);
    }

    private void SetupOptions()
    {
        // Remove all currently created options on the dropdown.
        dropdown.ClearOptions();


        List<TMP_Dropdown.OptionData> newOptions = new List<TMP_Dropdown.OptionData>();

        // Loop through all Quality settings
        foreach(string quality in QualitySettings.names)
        {
            // create a new option on the name and add it to the dropdown
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(quality);
            newOptions.Add(option);
        }

        // Add all the new options to the dropdown
        dropdown.AddOptions(newOptions);
        //Set the current value to the current quality level and show the correct value
        dropdown.value = QualitySettings.GetQualityLevel();
        dropdown.RefreshShownValue();
    }

    // Apply the quality setting to the game based on the new value of the dropdown
    private void OnOptionChanged(int _option) => QualitySettings.SetQualityLevel(_option);

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GraphicsQuality : MonoBehaviour
{
    public Dropdown dropdown;

    void Start()
    {
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(dropdown); });
        SetDefaultQuality();
    }

    void PopulateDropdown()
    {
        dropdown.ClearOptions();

        string[] qualityNames = QualitySettings.names;
        dropdown.AddOptions(new List<string>(qualityNames));
    }

    void SetDefaultQuality()
    {
        int defaultQualityIndex = PlayerPrefs.GetInt("GraphicsQuality", -1); // Get the saved quality level index
        if (defaultQualityIndex != -1)
        {
            dropdown.value = defaultQualityIndex;
            QualitySettings.SetQualityLevel(defaultQualityIndex, true);
        }
        else
        {
            Debug.LogWarning("Saved quality level not found. Setting to Ultra by default.");
            int ultraIndex = FindQualityIndex("Ultra");
            if (ultraIndex != -1)
            {
                dropdown.value = ultraIndex;
                QualitySettings.SetQualityLevel(ultraIndex, true);
            }
            else
            {
                Debug.LogWarning("Ultra quality level not found.");
            }
        }
    }

    int FindQualityIndex(string qualityName)
    {
        string[] qualityNames = QualitySettings.names;
        for (int i = 0; i < qualityNames.Length; i++)
        {
            if (qualityNames[i] == qualityName)
            {
                return i;
            }
        }
        return -1;
    }

    void OnDropdownValueChanged(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value, true);
        PlayerPrefs.SetInt("GraphicsQuality", dropdown.value); // Save the selected quality level index
    }
}

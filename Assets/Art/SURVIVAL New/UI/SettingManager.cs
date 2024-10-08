using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    // UI Elements
    public Dropdown resolutionDropdown; // Dropdown for resolution options
    public Dropdown qualityDropdown; // Dropdown for graphics quality
    public Toggle fullscreenToggle; // Toggle for fullscreen mode

    private Resolution[] resolutions;

    public GameObject SettingsPanel;

    void Start()
    {
        // Populate the resolution dropdown with available resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (
                resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height
            )
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load previously saved settings
        LoadSettings();
    }

    public void SaveSettings()
    {
        // Save resolution, quality, and fullscreen settings
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.SetInt("Quality", qualityDropdown.value);
        PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);

        // Apply the settings
        ApplySettings();

        // Save changes to disk
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        // Load resolution, quality, and fullscreen settings
        resolutionDropdown.value = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1);
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel());
        fullscreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        // Apply settings
        ApplySettings();
    }

    public void ApplySettings()
    {
        // Set resolution
        Resolution resolution = resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);

        // Set graphics quality
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        // Set fullscreen mode
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void RestoreDefaults()
    {
        // Restore default resolution and quality settings
        resolutionDropdown.value = resolutions.Length - 1;
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        fullscreenToggle.isOn = true;

        // Apply and save the default settings
        ApplySettings();
        SaveSettings();
    }

    public void OnSaveButtonClicked()
    {
        SaveSettings();
    }

    public void OnExitButtonClicked()
    {
        ClosePanel();
    }

    public void ClosePanel() {
        SettingsPanel.SetActive(false);
    }
}

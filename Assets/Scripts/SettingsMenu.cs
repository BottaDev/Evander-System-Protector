using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Toggle fullScreenToggle;

    private Resolution[] resolutions;
    private bool loadComplete = false;
    private int savedWidth = 0;
    private int savedHeight = 0;
    private bool savedFullScreen = false;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");

        savedWidth = PlayerPrefs.GetInt("ScreenWidth");
        savedHeight = PlayerPrefs.GetInt("ScreenHeight");
        savedFullScreen = PlayerPrefs.GetInt("FullScreen") == 1 ? true : false;

        if (savedWidth == 0 || savedHeight == 0)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
            savedWidth = Screen.currentResolution.width;
            savedHeight = Screen.currentResolution.height;
        }
        else
        {
            Screen.SetResolution(savedWidth, savedHeight, savedFullScreen);
        }

        GetResolutions();

        GetFullScreenMode();

        loadComplete = true;
    }

    private void GetFullScreenMode()
    {
        int mode = PlayerPrefs.GetInt("FullScreen");

        if (mode == 1)          // FullScreen on
        {
            Screen.fullScreen = true;
            fullScreenToggle.isOn = true;
        }
        else if (mode == 0)     // FullScreen off
        {
            Screen.fullScreen = false;
            fullScreenToggle.isOn = false;
        }
    }

    private void GetResolutions()
    {
        // All resolutions supported by the monitor (fullscreen)
        resolutions = Screen.resolutions.Distinct().ToArray();       
        resolutions = resolutions.Where(val => val.width >= 800).ToArray();
        resolutions = resolutions.Where(val => val.refreshRate == 60).ToArray();

        resolutionDropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            resolutionOptions.Add(option);

            if ((resolutions[i].width == savedWidth) && (resolutions[i].height == savedHeight))
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SaveOptions()
    {
        PlayerPrefs.Save();
    }

    // Runs when you select a resolution
    public void SetResolution()
    {
        if (loadComplete)
        {
            int resolutionIndex = resolutionDropdown.value;
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

            PlayerPrefs.SetInt("ScreenWidth", resolution.width);
            PlayerPrefs.SetInt("ScreenHeight", resolution.height);

            SaveOptions();
        }
    }

    // It is executed when modifying the volume bar
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        PlayerPrefs.SetFloat("Volume", volume);

        SaveOptions();
    }

    // It is executed by pressing the Fullscreen button
    public void SetFullscreen()
    {
        bool isFullScreen = fullScreenToggle.isOn;
        Screen.fullScreen = isFullScreen;

        if (isFullScreen)
            PlayerPrefs.SetInt("FullScreen", 1);
        else
            PlayerPrefs.SetInt("FullScreen", 0);

        SaveOptions();
    }
}

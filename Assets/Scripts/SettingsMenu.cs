using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    //private void Start()
    //{
    //    GetResolutions();

    //    GetFullScreenMode();

    //    volumeSlider.value = PlayerPrefs.GetFloat("Volume");

    //    int savedWidth = PlayerPrefs.GetInt("ScreenWidth");
    //    int savedHeight = PlayerPrefs.GetInt("ScreenHeight");

    //    Screen.SetResolution(savedWidth, savedHeight, Screen.fullScreen);
    //}

    //private void GetFullScreenMode()
    //{
    //    int mode = PlayerPrefs.GetInt("FullScreen");

    //    if (mode == 1)          // FullScreen activado
    //    {
    //        Screen.fullScreen = true;
    //        fullScreenToggle.isOn = true;
    //    } 
    //    else if (mode == 0)     // FullScreen desactivado
    //    {
    //        Screen.fullScreen = false;
    //        fullScreenToggle.isOn = false;
    //    }
    //}

    //private void GetResolutions()
    //{
    //    resolutions = Screen.resolutions;       // Todas las resoluciones soportadas por el monitor (fulllscreen)

    //    resolutionDropdown.ClearOptions();

    //    List<string> options = new List<string>();

    //    int currentResolutionIndex = 0;

    //    for (int i = 0; i < resolutions.Length; i++)
    //    {
    //        string option = resolutions[i].width + " x " + resolutions[i].height;

    //        options.Add(option);

    //        if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
    //            currentResolutionIndex = i;

    //        Debug.Log(resolutions[i]);
    //    }

    //    resolutionDropdown.AddOptions(options);
    //    resolutionDropdown.value = currentResolutionIndex;
    //    resolutionDropdown.RefreshShownValue();
    //}

    //private void SaveOptions()
    //{
    //    PlayerPrefs.Save();
    //}

    //// Se ejecuta al seleccionar una resolucion
    //public void SetResolution(int resolutionIndex)
    //{     
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

    //    PlayerPrefs.SetInt("ScreenWidth", resolution.width);
    //    PlayerPrefs.SetInt("ScreenHeight", resolution.height);

    //    SaveOptions();
    //}

    //// Se ejecuta al modificar la barra de volumen
    //public void SetVolume(float volume)
    //{
    //    audioMixer.SetFloat("Volume", volume);

    //    PlayerPrefs.SetFloat("Volume", volume);

    //    SaveOptions();
    //}

    //// Se ejecuta al apretar el boton de Fullscreen
    //public void SetFullscreen(bool isFullScreen)
    //{
    //    Screen.fullScreen = isFullScreen;

    //    if (isFullScreen)
    //        PlayerPrefs.SetInt("FullScreen", 1);
    //    else
    //        PlayerPrefs.SetInt("FullScreen", 0);

    //    SaveOptions();
    //}

}

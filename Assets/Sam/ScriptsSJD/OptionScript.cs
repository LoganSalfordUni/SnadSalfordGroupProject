using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionScript : MonoBehaviour
{
  
    public TMPro.TMP_Dropdown resolutionDropdown;
    
    Resolution[] _resolutions;
    
    
    // Video Settings 
    // all settings for video settings
    //---------------------------------------------------------------------------------
    void Start()
    {
       _resolutions = Screen.resolutions; 
       
       resolutionDropdown.ClearOptions();

       List<string> options = new List<string>();
       
       int currentResolutionIndex = 0;
       for (int i = 0; i < _resolutions.Length; i++)
       {
           string option = _resolutions[i].width + " x " + _resolutions[i].height + " @ " + _resolutions[i].refreshRate + "hz";
           options.Add(option);

           if (_resolutions[i].width == Screen.currentResolution.width &&
               _resolutions[i].height == Screen.currentResolution.height)
           {
               currentResolutionIndex = i;
           }
       }

       resolutionDropdown.AddOptions(options);
       resolutionDropdown.value = currentResolutionIndex;
       resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void FullscreenMode()
    {
        // Toggle Full screen by clicking the button
        Screen.fullScreen = !Screen.fullScreen;
    }

    //---------------------------------------------------------------------------------
    // Audio Settings
    [SerializeField] Slider volumeSlider;

    public void Awake()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume",1);
            Load();
        }

        else
        {
            Load();
        }
    }


    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }
    
    //---------------------------------------------------------------------------------
    //Controls Settings
    // Will add when I can bind properly
    //---------------------------------------------------------------------------------
    // accessibility Settings

    public void DyslexiaEnabler()
    {
        SceneManager.LoadScene("DyslexiaON");
        Debug.Log("Successfully Turned on Dyslexia Mode!");
        // When turned on should switch to the same menu but with the Dyslexia Font changed over in the menu
    }

    public void DyslexiaDisabler()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Successfully turned off Dyslexia Mode!");
        // When turned off will revert to default setting given when loading in the menu originally!
    }
    
}

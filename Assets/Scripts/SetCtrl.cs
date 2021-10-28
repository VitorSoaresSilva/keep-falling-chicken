using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetCtrl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public Dropdown resolutionDropdown;
    public Toggle tgWindow;

    Resolution[] resolutions;

    void Start()
    {
        mixer.SetFloat("volume", slider.value);

        resolutions = Screen.resolutions;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
        }

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if((resolutions[i].width == Screen.currentResolution.width) &&(resolutions[i].height == Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }

        if (resolutionDropdown != null)
        {
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

    }

    public void SetWindowMode()
    {
        if(tgWindow.isOn)
        {
            Screen.fullScreen = false;
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void AjustaVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }

    /*public void Resolution01()
    {
        Screen.SetResolution(1366, 768, true);
    }

    public void Resolution02()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    public void Resolution03()
    {
        Screen.SetResolution(2560, 1440, true);
    }
    public void Resolution04()
    {
        Screen.SetResolution(3840, 2160, true);
    }




    public void Graficos01()
    {
        QualitySettings.SetQualityLevel(0, true);
    }

    public void Graficos02()
    {
        QualitySettings.SetQualityLevel(1, true);
    }

    public void Graficos03()
    {
        QualitySettings.SetQualityLevel(2, true);
    }

    public void Graficos04()
    {
        QualitySettings.SetQualityLevel(3, true);
    }

    public void Graficos05()
    {
        QualitySettings.SetQualityLevel(4, true);
    }

    public void Graficos06()
    {
        QualitySettings.SetQualityLevel(5, true);
    }
    */
}

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;
public class ConfigView : BaseView
{
    public UnityEvent OnExitClicked;
    public Slider effectsVolume;
    public Slider musicVolume;


    public AudioMixer AudioMixer;
    private void Start()
    {
        musicVolume.SetValueWithoutNotify(GameManager.instance.ConfigData.musicVolume);
        effectsVolume.SetValueWithoutNotify(GameManager.instance.ConfigData.effectsVolume);
    }

    public void ClickExit()
    {
        OnExitClicked?.Invoke();
    }

    public void OnVolumeChanged()
    {
        GameManager.instance.ConfigData.effectsVolume = effectsVolume.value;
        GameManager.instance.ConfigData.musicVolume = musicVolume.value;
        GameManager.instance.SetVolumes();
    }
    
}

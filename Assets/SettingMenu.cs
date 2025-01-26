using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject OptionPanel;
    public AudioMixer audioMixer;
    
    
    public void SetVolume(float Volume)
    {
        audioMixer.SetFloat("Volume",Volume);
    }
    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void Back()
    {
       OptionPanel.SetActive(false);
        PausePanel.SetActive(true);


    }
}

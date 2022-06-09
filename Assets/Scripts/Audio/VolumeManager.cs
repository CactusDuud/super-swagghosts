using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string VolumePref = "VolumePref";
    private int firstPlayInt;
    public Slider volumeSlider;
    private float volumeFloat;
    public AudioSource BGM;

    void Start()
    {
        if (firstPlayInt == 0)
        {
            volumeFloat = .125f;
            volumeSlider.value = volumeFloat;
            PlayerPrefs.SetFloat(VolumePref, volumeFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else //if not first playthrough
        {
            volumeFloat = PlayerPrefs.GetFloat(VolumePref);
            volumeSlider.value = volumeFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(VolumePref, volumeSlider.value);
    }

    /*void OnApplicationFocus(bool inFocus) 
    {
        if (!inFocus) 
        {
            SaveSoundSettings();
        }
    }*/

    public void UpdateSound()
    {
        BGM.volume = volumeSlider.value;
    }
}

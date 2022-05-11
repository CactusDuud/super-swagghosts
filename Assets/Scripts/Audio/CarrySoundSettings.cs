using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrySoundSettings : MonoBehaviour
{
    private static readonly string VolumePref = "VolumePref";
    private float volumeFloat;
    public AudioSource BGM;

    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        volumeFloat = PlayerPrefs.GetFloat(VolumePref);
        BGM.volume = volumeFloat;

    }

}

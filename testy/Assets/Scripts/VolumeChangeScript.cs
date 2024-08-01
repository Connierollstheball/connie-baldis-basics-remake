using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeChangeScript : MonoBehaviour
{
    public Slider VolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1f);
        }

        LoadValues();
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Save();
    }

    public void LoadValues()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        AudioListener.volume = VolumeSlider.value;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", VolumeSlider.value);
    }
}

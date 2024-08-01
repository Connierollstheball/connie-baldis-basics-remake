using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSensitivityChangeScript : MonoBehaviour
{
    public Slider CSSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("cameraSensitivity"))
        {
            PlayerPrefs.SetFloat("cameraSensitivity", 1f);
        }

        LoadValues();
    }

    public void LoadValues()
    {
        CSSlider.value = PlayerPrefs.GetFloat("cameraSensitivity");
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("cameraSensitivity", CSSlider.value);
    }
}

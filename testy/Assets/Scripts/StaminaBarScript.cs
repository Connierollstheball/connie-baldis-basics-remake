using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    public GameObject Player;
    public Slider staminabar;

    void Update()
    {
        staminabar.value = (Player.GetComponent<PlayerScript>().sprint / 100);
    }
}

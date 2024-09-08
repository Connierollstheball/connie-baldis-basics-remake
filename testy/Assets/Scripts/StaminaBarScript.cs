using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarScript : MonoBehaviour
{
    GameObject Player;
    public GameObject GameController;
    public Slider staminabar;

    void Start()
    {
        // Game Objects as defined in Game Controller --------------------------------
        Player = GameController.GetComponent<GameControllerScript>().Player;
        // ---------------------------------------------------------------------------
    }

    void Update()
    {
        staminabar.value = (Player.GetComponent<PlayerScript>().sprint / 100);
    }
}

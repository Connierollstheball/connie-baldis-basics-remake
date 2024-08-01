using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsTrigger : MonoBehaviour
{
    public GameObject Canvas;

    // Settings Trigger Protocol ------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Test");
            Canvas.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    //---------------------------------------------------

    public void OnExitSettings()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

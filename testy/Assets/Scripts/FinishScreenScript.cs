using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScreenScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Quitting");
            Application.Quit();
        }
    }
}

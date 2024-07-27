using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bluedoorscript : MonoBehaviour
{
    public GameObject DoorOne;
    public GameObject DoorTwo;
    public Material DoorOpen;
    public Material DoorClose;
    public AudioClip AudioOpen;
    public AudioClip AudioClose;
    public AudioSource AudioSource;
    public GameObject DoorObject;

	public void open()
    {
        CancelInvoke("close");
        DoorOne.GetComponent<Renderer>().material = DoorOpen;
        DoorTwo.GetComponent<Renderer>().material = DoorOpen;

        DoorOne.GetComponent<Collider>().enabled = false;
        DoorTwo.GetComponent<Collider>().enabled = false;

        DoorOne.layer = 2;
        DoorTwo.layer = 2;

        AudioSource.PlayOneShot(AudioOpen);
        Invoke("close", 2.0f);
    }

    public void close()
    {
        CancelInvoke("close");
        DoorOne.GetComponent<Renderer>().material = DoorClose;
        DoorTwo.GetComponent<Renderer>().material = DoorClose;   

        DoorOne.GetComponent<Collider>().enabled = true;
        DoorTwo.GetComponent<Collider>().enabled = true;

        DoorOne.layer = 0;
        DoorTwo.layer = 0;

        AudioSource.PlayOneShot(AudioClose);  
    }
}

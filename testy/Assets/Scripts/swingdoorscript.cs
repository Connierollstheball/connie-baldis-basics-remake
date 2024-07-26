using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swingdoorscript : MonoBehaviour
{
    public GameObject SwingingDoor;
    public GameObject SwingDoorPartOne;
    public GameObject SwingDoorPartTwo;
    public Material DoorOpen;
    public Material DoorClose;
    public Material DoorLock;
    public AudioClip AudioOpen;
    public AudioClip AudioClose;
    public AudioClip AudioLock;
    public AudioSource AudioSource;
    public bool lockedDoor = false;
    public bool somethinginside;
    public GameObject NavMeshObstacle;

    void Start()
    {
        NavMeshObstacle.SetActive(false);
    }

	public void open()
    {
        if (lockedDoor == false) 
        {
            CancelInvoke("close");
            SwingDoorPartOne.GetComponent<Renderer>().material = DoorOpen;
            SwingDoorPartTwo.GetComponent<Renderer>().material = DoorOpen;

            SwingDoorPartOne.GetComponent<Collider>().enabled = false;
            SwingDoorPartTwo.GetComponent<Collider>().enabled = false;

            AudioSource.PlayOneShot(AudioOpen);
            Invoke("close", 2.0f);
        }
    }

    public void close()
    {
        if (lockedDoor == false)
        {
            if (somethinginside == false)
            {
                CancelInvoke("close");
                SwingDoorPartOne.GetComponent<Renderer>().material = DoorClose;
                SwingDoorPartTwo.GetComponent<Renderer>().material = DoorClose;   

                SwingDoorPartOne.GetComponent<Collider>().enabled = true;
                SwingDoorPartTwo.GetComponent<Collider>().enabled = true;

                AudioSource.PlayOneShot(AudioClose);  
            }
            else
            {
                Invoke("close", 2.0f);
            }
        }
    }

    public void lockDoor()
    {
        CancelInvoke("close");
        Invoke("unlockDoor", 6.0f);
        SwingDoorPartOne.GetComponent<Renderer>().material = DoorLock;
        SwingDoorPartTwo.GetComponent<Renderer>().material = DoorLock;

        SwingDoorPartOne.GetComponent<Collider>().enabled = true;
        SwingDoorPartTwo.GetComponent<Collider>().enabled = true;
        AudioSource.PlayOneShot(AudioLock);
        lockedDoor = true;

        NavMeshObstacle.SetActive(true);
    }

    public void unlockDoor()
    {
        CancelInvoke("unlockDoor");
        lockedDoor = false;
        SwingDoorPartOne.GetComponent<Renderer>().material = DoorClose;
        SwingDoorPartTwo.GetComponent<Renderer>().material = DoorClose;  

        SwingDoorPartOne.GetComponent<Collider>().enabled = false;
        SwingDoorPartTwo.GetComponent<Collider>().enabled = false;
        AudioSource.PlayOneShot(AudioClose); 

        NavMeshObstacle.SetActive(false);
    }
}

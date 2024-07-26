using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlacefaceScript : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        // Player Targetting ----------------------------------------------------
        Agent.destination = Player.transform.position;
        //-------------------------------------------------------------------
    }

    private void OnTriggerEnter(Collider other)
    {
        // Swinging Doors ---------------------------------------------------
        if (other.tag == "SwingDoor")
        {
            other.GetComponent<swingdoorscript>().open();
            other.GetComponent<swingdoorscript>().somethinginside = true;
        }
        //-------------------------------------------------------------------

        // Blue Doors -------------------------------------------------------
        if (other.tag == "BlueDoor")
        {
            other.transform.parent.gameObject.GetComponent<bluedoorscript>().open();
        }
        //-------------------------------------------------------------------
    }

    private void OnTriggerExit(Collider other)
    {
        // Swinging Doors ---------------------------------------------------
        if (other.tag == "SwingDoor")
        {
            other.GetComponent<swingdoorscript>().somethinginside = false;
        }
        //-------------------------------------------------------------------
    }
}

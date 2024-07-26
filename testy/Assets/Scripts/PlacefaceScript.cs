using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlacefaceScript : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Player;
    public GameObject Enemy;
    public bool seesPlayer;

    // Update is called once per frame
    void Update()
    {
        // Player Targetting ------------------------------------------------
        Agent.destination = Player.transform.position;
        //-------------------------------------------------------------------

        // Character's Vision (chracter will look for the player) -----------
        RaycastHit hit;
        Vector3 direction = (Player.transform.position - Enemy.transform.position).normalized;
        Ray ray = new Ray(Enemy.transform.position, direction);

        if (Physics.Raycast(ray, out hit))
        {
            // Does the Enemy see you? --------------------------------------
            if (hit.collider.transform.gameObject == Player)
            {
                seesPlayer = true;
            }
            else
            {
                seesPlayer = false;
            }
            //---------------------------------------------------------------

            // Getting Caught in the Principal's Room ----------------------
            if (seesPlayer == true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().guiltval == 1)
            {
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                Agent.speed = 10f;
            }
            //---------------------------------------------------------------
        }
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

        // Hitting Player ---------------------------------------------------
        if (other.gameObject == Player && seesPlayer == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

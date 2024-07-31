using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BaldiScript : MonoBehaviour
{
    public GameObject Baldi;
    public GameObject Player;
    public NavMeshAgent Agent;
    public LayerMask layerstohit;
    public bool seesPlayer = false;
    public AudioSource AudioSource;
    public AudioClip SlapSound;
    public GameObject GameController;

    // Start is called before the first frame update
    void Start()
    {
        // Start the slaps
        Agent.speed = 0f;
        SlapLoop();
        GameObject WanderPoint = GameController.GetComponent<GameControllerScript>().chooseWanderPoint();
        Agent.destination = WanderPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Temporary Destination
        //Agent.destination = Player.transform.position;

        // Character's Vision (chracter will look for the player) -----------
        RaycastHit hit;
        Vector3 direction = (Player.transform.position - Baldi.transform.position).normalized;
        Ray ray = new Ray(Baldi.transform.position, direction);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerstohit))
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
        }

        // Player Spotted ---------------------------------------------------
        if (seesPlayer == true)
        {
            Agent.destination = Player.transform.position;
        }
        //-------------------------------------------------------------------


        // Player not at destination? Wander again ---------------------------
        if (Agent.remainingDistance <= Agent.stoppingDistance && seesPlayer == false)
        {
            GameObject WanderPoint = GameController.GetComponent<GameControllerScript>().chooseWanderPoint();
            Agent.destination = WanderPoint.transform.position;
        }
        //-------------------------------------------------------------------
    }

    // Slap Logic -------------------------------------------
    // Slap Looping
    void SlapLoop()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("slap");
        Slap();
        Invoke("SlapLoop", 2.0f);
    }

    // Slapping
    void Slap()
    {
        AudioSource.PlayOneShot(SlapSound);
        Agent.speed = 25f;
        Invoke("onSlapDone", 0.25f);
    }

    // Baldi Stops
    void onSlapDone()
    {
        Agent.speed = 0f;
    }
    //-------------------------------------------------------

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

        // Principal's Door ------------------------------------------------
        if (other.tag == "PrincipalDoor" && Player.GetComponent<PlayerScript>().JailTime > 0f)
        {
            other.transform.parent.gameObject.GetComponent<bluedoorscript>().forceOpen();
        }
        //-------------------------------------------------------------------

        // Player -----------------------------------------------------------
        if (seesPlayer == true && other.gameObject == Player)
        {
            // Make the player look at Baldi
            Vector3 targetPosition = new Vector3(this.gameObject.transform.position.x, Player.transform.transform.position.y, this.gameObject.transform.position.z);
            Player.transform.LookAt(targetPosition);

            // Stop the player and Baldi from being able to move
            Destroy(Player.GetComponent<Rigidbody>());
            Destroy(Baldi.GetComponent<Rigidbody>());
            CancelInvoke("SlapLoop");
            Agent.enabled = false;

            // Change this variable and reverse any camera flipping
            Player.GetComponent<PlayerScript>().caught = true;

            if (Player.GetComponent<PlayerScript>().cameraflipped == true)
            {
                Player.GetComponent<PlayerScript>().camera.transform.rotation = Player.GetComponent<PlayerScript>().camera.transform.rotation * Quaternion.Euler(0, 180, 0);
            }

            // You failed!
            Invoke("onGameDone", 1.0f);
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

    public void BaldiSetDestination(GameObject pointgo)
    {
        Agent.destination = pointgo.transform.position;
    }

    void onGameDone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

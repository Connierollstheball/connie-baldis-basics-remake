using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlacefaceScript : MonoBehaviour
{
    public NavMeshAgent Agent;
    GameObject Player;
    public GameObject GameController;
    public GameObject Enemy;
    public bool seesPlayer;
    public LayerMask layerstohit;
    public AudioSource AudioSource;
    public AudioClip noFaculty;
    public AudioClip noRunning;
    public AudioClip noDrink;
    public GameObject TPPlayer;
    public GameObject TPSelf;
    public bool ignoringPlayer = false;

    void Start()
    {
        // Game Objects as defined in Game Controller --------------------------------
        Player = GameController.GetComponent<GameControllerScript>().Player;
        // ---------------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        // Player Targetting ---------------------------------------------------------------------------------------------------------
        if (ignoringPlayer != true) 
        {
            Agent.destination = Player.transform.position;
        }

        // If the placeface is back to his original spot after sending the player to detention, release him ------------------------
        if (ignoringPlayer == true && this.gameObject.transform.position.x == 67.0f && this.gameObject.transform.position.z == 20.0f)
        {
            ignoringPlayer = false;
            Agent.speed = 3.5f;
        }
        //--------------------------------------------------------------------------------------------------------------------------

        // Character's Vision (chracter will look for the player) -----------
        RaycastHit hit;
        Vector3 direction = (Player.transform.position - Enemy.transform.position).normalized;
        Ray ray = new Ray(Enemy.transform.position, direction);

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

            // Getting Caught in the Principal's Room ----------------------
            if (seesPlayer == true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().insideofFaculty == true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false)
            {
                ignoringPlayer = false;
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                Agent.speed = 10f;
                AudioSource.PlayOneShot(noFaculty);
            }
            //---------------------------------------------------------------

            // No running in the halls --------------------------------------
            if (seesPlayer == true && Player.GetComponent<PlayerScript>().multiplier == 20f && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false)
            {
                ignoringPlayer = false;
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().guiltval += 0.01f;

                if (hit.collider.transform.gameObject.GetComponent<PlayerScript>().guiltval >= 0.75f)
                {
                    hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                    Agent.speed = 10f;
                    AudioSource.PlayOneShot(noRunning);
                }
            }
            //---------------------------------------------------------------
        }
        //-------------------------------------------------------------------
    }

    // No Drinking Drinks in the halls -----------------------------------------
    public void checkifseeBSODA()
    {
        RaycastHit hit;
        Vector3 direction = (Player.transform.position - Enemy.transform.position).normalized;
        Ray ray = new Ray(Enemy.transform.position, direction);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerstohit) && hit.collider.transform.gameObject == Player && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false)
        {
            ignoringPlayer = false;
            hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
            Agent.speed = 10f;
            AudioSource.PlayOneShot(noDrink);
        }
    }
    //--------------------------------------------------------------------------

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
            // Detention ---- Another Principal Function ---------------
            // TODO: Complete this
            if (Player.GetComponent<PlayerScript>().lockedinguilt == true)
            {
                Player.transform.position = TPPlayer.transform.position;
                this.gameObject.transform.position = TPSelf.transform.position;
                Agent.speed = 0f;
                ignoringPlayer = true;
                this.gameObject.GetComponent<Rigidbody>().velocity = this.gameObject.GetComponent<Rigidbody>().velocity * -1;
                Player.GetComponent<PlayerScript>().lockedinguilt = false;
                Player.GetComponent<PlayerScript>().guiltval = 0f;
                Invoke("gotowardsPoint", 2f);
            }
            //-----------------------------------------------------------
            else
            {
                if (ignoringPlayer != true)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }
        //-------------------------------------------------------------------
    }

    // Speed when returning to original point --------------------------
    void gotowardsPoint()
    {
        Agent.speed = 5f;
        Agent.destination = new Vector3(67f, 0f, 20f);
    }
    //-------------------------------------------------------------------

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

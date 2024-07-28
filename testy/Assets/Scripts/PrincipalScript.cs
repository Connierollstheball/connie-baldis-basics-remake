using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PrincipalScript : MonoBehaviour
{
    public NavMeshAgent Agent;
    public GameObject Player;
    public AudioClip noFaculty;
    public AudioClip noRunning;
    public AudioClip noDrink;
    public AudioClip noEscape;
    public GameObject Enemy;
    public AudioSource AudioSource;
    public bool seesPlayer;
    public LayerMask layerstohit;
    public GameObject TPPlayer;
    public GameObject TPSelf;
    public GameObject GameController;
    public GameObject PrincipalDoor;

    void Start()
    {
        GameObject AIPoint = GameController.GetComponent<GameControllerScript>().chooseWanderPoint();
        Agent.destination = AIPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Player Targetting ---------------------------------------------------------------------------------------------------------
        if (Player.GetComponent<PlayerScript>().lockedinguilt == true)
        {
            Agent.destination = Player.transform.position;
        }

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            if (Player.GetComponent<PlayerScript>().lockedinguilt != true)
            {
                GameObject AIPoint = GameController.GetComponent<GameControllerScript>().chooseWanderPoint();
                Agent.destination = AIPoint.transform.position;
            }
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
            if (seesPlayer == true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().insideofFaculty == true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false && Player.GetComponent<PlayerScript>().JailTime <= 0.1f)
            {
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                Agent.speed = 20f;
                AudioSource.PlayOneShot(noFaculty);
            }
            //---------------------------------------------------------------

            // No running in the halls --------------------------------------
            if (seesPlayer == true && Player.GetComponent<PlayerScript>().multiplier == 20f && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false && Player.GetComponent<PlayerScript>().JailTime <= 0.1f)
            {
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().guiltval += 0.01f;

                if (hit.collider.transform.gameObject.GetComponent<PlayerScript>().guiltval >= 0.75f)
                {
                    hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                    Agent.speed = 20f;
                    AudioSource.PlayOneShot(noRunning);
                }
            }
            //---------------------------------------------------------------

            // No escaping detention in the halls ---------------------------
            if (seesPlayer == true && Player.GetComponent<PlayerScript>().JailTime > 0.1f && Player.GetComponent<PlayerScript>().inJailTrigger != true && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false)
            {
                hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
                Agent.speed = 20f;
                AudioSource.PlayOneShot(noEscape);
            }
            //---------------------------------------------------------------
        }
    }

    // No Drinking Drinks in the halls -----------------------------------------
    public void checkifseeBSODA()
    {
        RaycastHit hit;
        Vector3 direction = (Player.transform.position - Enemy.transform.position).normalized;
        Ray ray = new Ray(Enemy.transform.position, direction);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerstohit) && hit.collider.transform.gameObject == Player && hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt == false && Player.GetComponent<PlayerScript>().JailTime <= 0.1f)
        {
            hit.collider.transform.gameObject.GetComponent<PlayerScript>().lockedinguilt = true;
            Agent.speed = 20f;
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

        // His Door ---------------------------------------------------------
        if (other.tag == "PrincipalDoor" && Player.GetComponent<PlayerScript>().JailTime > 0f)
        {
            other.transform.parent.gameObject.GetComponent<bluedoorscript>().forceOpen();
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
                // Why the fuck was the NavMeshAgent even interfering with this shit???????
                Agent.enabled = false;
                Enemy.transform.position = TPSelf.transform.position;
                Agent.enabled = true;
                Agent.speed = 0f;
                Vector3 targetPosition = new Vector3(this.gameObject.transform.position.x, Player.transform.transform.position.y, this.gameObject.transform.position.z);
                Player.transform.LookAt(targetPosition);
                Player.GetComponent<PlayerScript>().lockedinguilt = false;
                Player.GetComponent<PlayerScript>().guiltval = 0f;
                Player.GetComponent<PlayerScript>().JailTime = 15f;
                PrincipalDoor.GetComponent<bluedoorscript>().lockDoor();
                Enemy.transform.position = TPSelf.transform.position;
                Invoke("gotowardsPoint", 2f);
            }
            //-----------------------------------------------------------
        }
        //-------------------------------------------------------------------
    }

    // Speed when returning to original point --------------------------
    void gotowardsPoint()
    {
        GameObject AIPoint = GameController.GetComponent<GameControllerScript>().chooseWanderPoint();
        Agent.speed = 15f;
        Agent.destination = AIPoint.transform.position;
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

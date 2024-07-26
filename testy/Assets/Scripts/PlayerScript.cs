using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Camera camera;
    float multiplier;
    public float sprint = 100f;
    public float guiltval = 0f;
    public bool lockedinguilt = false;

    void Update()
    {
        float xmov = 0f;
        float zmov = 0f;

        //Other Move Logic ----------------------------------------------------------
        if (Input.GetKey(KeyCode.W))
        {
            zmov = 1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            zmov = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xmov = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            xmov = 1f;
        }
        //------------------------------------------------------------------------

        //Spring & Move Logic ----------------------------------------------------
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (sprint >= 1f)
            {
                multiplier = 20f;
            }
            else
            {
                multiplier = 10f;
            }

            if (rb.velocity.magnitude != 0)
            {
                sprint -= 0.1f;
            }
            else
            {
                sprint += 0.1f;
            }
        }
        else
        {
            multiplier = 10f;

            if (sprint < 100f && rb.velocity.magnitude == 0) 
            {
                sprint += 0.1f;
            }
        }

        if (sprint < -1f)
        {
            sprint = 0f;
        }

        Vector3 direction = new Vector3(xmov, 0, zmov).normalized;
        Vector3 rotatedDirection = camera.transform.TransformDirection(direction);

        rb.velocity = rotatedDirection * multiplier;
        //-------------------------------------------------------------------

        //Detect Clicks -----------------------------------------------------
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Locking Swing Doors - to be the Lock Item
                if (hit.collider.tag == "SwingDoor" && hit.distance < 7.0f && hit.collider.gameObject.GetComponent<swingdoorscript>().lockedDoor == false)
                {
                    hit.collider.gameObject.GetComponent<swingdoorscript>().lockDoor();
                }

                // Blue Doors
                if (hit.collider.tag == "BlueDoor" && hit.distance < 7.0f)
                {
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<bluedoorscript>().open();
                }
            }
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

        // Entering Faculty Rooms -------------------------------------------
        if (other.tag == "FacultyTrigger")
        {
            guiltval = 1f;
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

        // Exiting Faculty Rooms -------------------------------------------
        if (other.tag == "FacultyTrigger" && lockedinguilt == false)
        {
            guiltval = 0f;
        }
        //-------------------------------------------------------------------
    }
}
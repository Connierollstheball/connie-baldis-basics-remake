using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSODASprayScript : MonoBehaviour
{
    // This unfortunately is not as good at the original BBasics one, but I tried.
    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = transform.forward * 12f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SwingDoor")
        {
            other.GetComponent<swingdoorscript>().open();
        }

        if (other.tag == "BlueDoor")
        {
            other.transform.parent.gameObject.GetComponent<bluedoorscript>().open();
        }

        if (other.tag == "EnemyTag")
        {

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyTag")
        {
            other.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity + other.GetComponent<Rigidbody>().velocity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EnemyTag")
        {
            other.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}

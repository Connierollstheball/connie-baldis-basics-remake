using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSODASprayScript : MonoBehaviour
{
    // This unfortunately is not as good at the original BBasics one, but I tried.
    public Vector3 inittransform;
    public float lifeSpan;

    void Start()
    {
        inittransform = transform.forward;
    }

    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = inittransform * 12f;
        lifeSpan += 0.01f;

        if (lifeSpan > 30f)
        {
            Destroy(this.gameObject);
        }
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

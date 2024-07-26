using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{

    public int idofpickup;
    public GameObject item;
    private float initialposition;
    private float offsetaccel = 0.005f;
    private bool goingup;

    void Start()
    {
        initialposition = item.transform.position.y;
        goingup = false;
    }

    // Make it go up & down --------------------------------------------------------------------------------------
    void Update()
    {
        if (item.transform.position.y > initialposition - 0.25f)
        {
            if (goingup == false) 
            {
                float newy = item.transform.position.y - offsetaccel;
                item.transform.position = new Vector3(item.transform.position.x, newy, item.transform.position.z);
            }
        }
        else
        {
            goingup = true;
        }

        if (item.transform.position.y < initialposition + 0.25f)
        {
            if (goingup == true)
            {
                float newy = item.transform.position.y + offsetaccel;
                item.transform.position = new Vector3(item.transform.position.x, newy, item.transform.position.z);
            }
        }
        else
        {
            goingup = false;
        }
    }
}

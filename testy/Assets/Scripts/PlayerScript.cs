using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public Camera camera;
    float multiplier;
    public float sprint = 100f;
    public float guiltval = 0f;
    public bool lockedinguilt = false;
    public int item1 = 0;
    public int item2 = 0;
    public int item3 = 0;
    public int selecteditem = 0;
    public GameObject item1slot;
    public GameObject item2slot;
    public GameObject item3slot;
    public GameObject item1slotbackg;
    public GameObject item2slotbackg;
    public GameObject item3slotbackg;
    public Sprite YellowDoorLockSprite;
    public Sprite ZestySprite;
    public Text ItemText;

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
                // Blue Doors
                if (hit.collider.tag == "BlueDoor" && hit.distance < 8.0f)
                {
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<bluedoorscript>().open();
                }

                //Detect Item Collection
                if (hit.collider.tag == "ItemTag" && hit.distance < 8.0f)
                {
                    CheckInventoryPutObject(hit.collider.gameObject);
                }
            }
        }
        //-------------------------------------------------------------------

        // Item Uses --------------------------------------------------------------
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Yellow Door Lock
                if (hit.collider.tag == "SwingDoor" && hit.distance < 8.0f && hit.collider.gameObject.GetComponent<swingdoorscript>().lockedDoor == false && checkifItem(1))
                {
                    hit.collider.gameObject.GetComponent<swingdoorscript>().lockDoor();
                    removeItem(selecteditem);
                }
            }

            // Energy Flavored Zesty Bar
            if (checkifItem(2))
            {
                sprint = 150f;
                removeItem(selecteditem);
            }
        }

        // Inventory Selection ----------------------------------------------------
        if (Input.GetKey(KeyCode.Alpha1))
        {
            selecteditem = 0;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            selecteditem = 1;
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            selecteditem = 2;
        }
        //------------------------------------------------------------------------

        // UI Item Slot Updating -------------------------------------------------
        // TODO: Find a better way to do this? All of the Item Stuff?
        switch (item1)
        {
            case 0:
            item1slot.SetActive(false);
            break;
            case 1:
            item1slot.SetActive(true);
            item1slot.GetComponent<Image>().sprite = YellowDoorLockSprite;
            break;
            case 2:
            item1slot.SetActive(true);
            item1slot.GetComponent<Image>().sprite = ZestySprite;
            break;
        }

        switch (item2)
        {
            case 0:
            item2slot.SetActive(false);
            break;
            case 1:
            item2slot.SetActive(true);
            item2slot.GetComponent<Image>().sprite = YellowDoorLockSprite;
            break;
            case 2:
            item2slot.SetActive(true);
            item2slot.GetComponent<Image>().sprite = ZestySprite;
            break;
        }

        switch (item3)
        {
            case 0:
            item3slot.SetActive(false);
            break;
            case 1:
            item3slot.SetActive(true);
            item3slot.GetComponent<Image>().sprite = YellowDoorLockSprite;
            break;
            case 2:
            item3slot.SetActive(true);
            item3slot.GetComponent<Image>().sprite = ZestySprite;
            break;
        }

        switch (selecteditem)
        {
            case 0:
            item1slotbackg.GetComponent<Image>().color = Color.red;
            item2slotbackg.GetComponent<Image>().color = Color.white;
            item3slotbackg.GetComponent<Image>().color = Color.white;
            break;

            case 1:
            item1slotbackg.GetComponent<Image>().color = Color.white;
            item2slotbackg.GetComponent<Image>().color = Color.red;
            item3slotbackg.GetComponent<Image>().color = Color.white;
            break;

            case 2:
            item1slotbackg.GetComponent<Image>().color = Color.white;
            item2slotbackg.GetComponent<Image>().color = Color.white;
            item3slotbackg.GetComponent<Image>().color = Color.red;
            break;
        }

        doItemText();
    }

    void doItemText()
    {
        if (checkifItem(0))
        {
            ItemText.GetComponent<Text>().text = "Nothing";
        }
        else if (checkifItem(1))
        {
            ItemText.GetComponent<Text>().text = "Yellow Door Lock";
        }
        else if (checkifItem(2))
        {
            ItemText.GetComponent<Text>().text = "Zesty Bar";
        }
    }
    //------------------------------------------------------------------------

    // Called when an Item is used -----------------------------------------------
    void removeItem(int i)
    {
        if (i == 0)
        {
            item1 = 0;
        }
        else if (i == 1)
        {
            item2 = 0;
        }
        else if (i == 2)
        {
            item3 = 0;
        }
    }
    //------------------------------------------------------------------------

    // Item checking protocol ---------------------------------------------------
    bool checkifItem(int i)
    {
        if (selecteditem == 0 && item1 == i)
        {
            return true;
        }
        else if (selecteditem == 1 && item2 == i)
        {
            return true;
        }
        else if (selecteditem == 2 && item3 == i)
        {
            return true;
        }

        return false;
    }
    //------------------------------------------------------------------------

    // Protocol to put Items in someone's inventory --------------------------
    void CheckInventoryPutObject(GameObject item)
    {
        bool foundspot = false;

        if (item1 == 0)
        {
            item1 = item.GetComponent<ItemScript>().idofpickup;
            foundspot = true;
        }

        if (item2 == 0 && foundspot != true)
        {
            item2 = item.GetComponent<ItemScript>().idofpickup;
            foundspot = true;
        }

        if (item3 == 0 && foundspot != true)
        {
            item3 = item.GetComponent<ItemScript>().idofpickup;
            foundspot = true;
        }

        if (foundspot != true)
        {
            if (selecteditem == 0)
            {
                item1 = item.GetComponent<ItemScript>().idofpickup;
            }
            else if (selecteditem == 1)
            {
                item2 = item.GetComponent<ItemScript>().idofpickup;
            }
            else if (selecteditem == 2)
            {
                item3 = item.GetComponent<ItemScript>().idofpickup;
            }
        }

        Destroy(item);
    }
    //------------------------------------------------------------------------

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
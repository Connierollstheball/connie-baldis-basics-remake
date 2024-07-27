using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //TODO: move a bunch of stuff over to GameController ------------------
    public GameObject GameController;
    //---------------------------------------------------------------------
    
    public Rigidbody rb;
    public Camera camera;
    public float multiplier;
    public float sprint = 100f;
    public float guiltval = 0f;
    public bool lockedinguilt = false;
    public int selecteditem = 0;
    int[] inventory = new int[3];
    public GameObject[] inventoryslots = new GameObject[3];
    public GameObject[] inventoryslotsbackg = new GameObject[3];
    public GameObject item1slot;
    public GameObject item2slot;
    public GameObject item3slot;
    public GameObject item1slotbackg;
    public GameObject item2slotbackg;
    public GameObject item3slotbackg;
    public Sprite YellowDoorLockSprite;
    public Sprite ZestySprite;
    public Sprite BSODASprite;
    public Text ItemText;
    public bool cameraflipped = false;
    public GameObject BSODAProjectile;
    public AudioClip SodaSpray;
    public AudioSource AudioSource;
    public bool insideofFaculty;

    void Start()
    {
        // Set Initial Inventory "Items" ---------------------------------------------
        inventory[0] = 0;
        inventory[1] = 0;
        inventory[2] = 0;

        inventoryslots[0] = item1slot;
        inventoryslots[1] = item2slot;
        inventoryslots[2] = item3slot;

        inventoryslotsbackg[0] = item1slotbackg;
        inventoryslotsbackg[1] = item2slotbackg;
        inventoryslotsbackg[2] = item3slotbackg;
        //----------------------------------------------------------------------------
    }

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
        Vector3 rotatedDirection;

        if (cameraflipped == false)
        {
            rotatedDirection = camera.transform.TransformDirection(direction);
        }
        else
        {
            rotatedDirection = camera.transform.TransformDirection(direction) * -1;
        }

        rb.velocity = rotatedDirection * multiplier;
        //-------------------------------------------------------------------

        // Guilt Value Cap --------------------------------------------------
        if (guiltval > 1f)
        {
            guiltval = 1f;
        }
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
                    if (hit.collider.gameObject.GetComponent<ItemScript>().idofpickup < 99) 
                    {
                        CheckInventoryPutObject(hit.collider.gameObject);
                    }
                    else
                    {
                        //Notebook protocol to go here
                    }
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
                sprint = 200f;
                removeItem(selecteditem);
            }

            //BSODA 
            if (checkifItem(3))
            {
                Instantiate(BSODAProjectile, camera.transform.position, camera.transform.rotation);
                AudioSource.PlayOneShot(SodaSpray);
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

        // UI Item Slot Updating ------- Update this 2 whenever you add new Items ------
        for (int i = 0; i < 3; i ++)
        {
            switch (inventory[i])
            {
                case 0:
                inventoryslots[i].SetActive(false);
                break;
                case 1:
                inventoryslots[i].SetActive(true);
                inventoryslots[i].GetComponent<Image>().sprite = YellowDoorLockSprite;
                break;
                case 2:
                inventoryslots[i].SetActive(true);
                inventoryslots[i].GetComponent<Image>().sprite = ZestySprite;
                break;         
                case 3:
                inventoryslots[i].SetActive(true);
                inventoryslots[i].GetComponent<Image>().sprite = BSODASprite;
                break;         
            }
        }

        for (int i = 0; i < 3; i ++)
        {
            if (inventoryslotsbackg[i] == inventoryslotsbackg[selecteditem])
            {
                inventoryslotsbackg[i].GetComponent<Image>().color = Color.red;
            }
            else
            {
                inventoryslotsbackg[i].GetComponent<Image>().color = Color.white;
            }
        }

        doItemText();
        //------------------------------------------------------------------------------

        // Camera flipping -------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraflipped = true;
            camera.transform.rotation = camera.transform.rotation * Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            cameraflipped = false;
            camera.transform.rotation = camera.transform.rotation * Quaternion.Euler(0, 180, 0);
        }
        //------------------------------------------------------------------------------
    }

    // Item Text Updating, update this too when new item ---------------------------
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
            ItemText.GetComponent<Text>().text = "Energy Flavored Zesty Bar";
        }
        else if (checkifItem(3))
        {
            ItemText.GetComponent<Text>().text = "BSODA";
        }
    }
    //------------------------------------------------------------------------------

    // Called when an Item is used -----------------------------------------------
    void removeItem(int i)
    {
        inventory[i] = 0;
    }
    //------------------------------------------------------------------------

    // Item checking protocol ---------------------------------------------------
    bool checkifItem(int i)
    {
        if (inventory[selecteditem] == i)
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

        for (int i = 0; i < 3; i ++)
        {
            if (inventory[i] == 0)
            {
                inventory[i] = item.GetComponent<ItemScript>().idofpickup;
                foundspot = true;
                break;
            }
        }

        if (foundspot != true)
        {
            switch (inventory[selecteditem])
            {
                case 1:
                Instantiate(GameController.GetComponent<GameControllerScript>().YellowLock, item.transform.position, item.transform.rotation);
                break;

                case 2:
                Instantiate(GameController.GetComponent<GameControllerScript>().Zesty, item.transform.position, item.transform.rotation);
                break;

                case 3:
                Instantiate(GameController.GetComponent<GameControllerScript>().BSODA, item.transform.position, item.transform.rotation);
                break;
            }

            inventory[selecteditem] = item.GetComponent<ItemScript>().idofpickup;
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
            insideofFaculty = true;
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
            insideofFaculty = false;
        }
        //-------------------------------------------------------------------
    }
}
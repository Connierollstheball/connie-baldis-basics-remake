//NOTICE: When you make a new level, put the new scene name in CheckIfLevel()!!

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Sprite KeySprite;
    public Text ItemText;
    public bool cameraflipped = false;
    public GameObject BSODAProjectile;
    public AudioClip SodaSpray;
    public AudioSource AudioSource;
    public bool insideofFaculty;
    public GameObject Principal;
    public float JailTime;
    public GameObject PrincipalDoor;
    public GameObject DetentionText;
    public bool inJailTrigger = false;
    public bool caught = false;
    public GameObject Baldi;

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

    void FixedUpdate()
    {
        // Detention Time ---------- Rest in Update ---------------------------------
        if (CheckIfLevel(SceneManager.GetActiveScene().name))
        {
            JailTime -= 0.025f;

            if (JailTime > 0f)
            {
                DetentionText.SetActive(true);
                DetentionText.GetComponent<Text>().text = "You have Detention!\n" + Mathf.Round(JailTime);
            }
        }
        //------------------------------------------------------------------------

        //Other Move Logic ----------------------------------------------------------
        float xmov = 0f;
        float zmov = 0f;

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
                sprint -= 0.25f;
            }
            else
            {
                sprint += 0.25f;
            }
        }
        else
        {
            multiplier = 10f;

            if (sprint < 100f && rb.velocity.magnitude == 0) 
            {
                sprint += 0.25f;
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
    }

    void Update()
    {
        // Guilt Value Cap --------------------------------------------------
        if (guiltval > 1f)
        {
            guiltval = 1f;
        }
        //-------------------------------------------------------------------

        // Quit App (to be replaced with pause menu) ------------------------
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        //-------------------------------------------------------------------

        //Detect Clicks -----------------------------------------------------
        if (Input.GetMouseButtonDown(0) && caught == false)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Blue Doors
                if (hit.collider.tag == "BlueDoor" && hit.distance < 8.0f)
                {
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<bluedoorscript>().open();
                    Baldi.GetComponent<BaldiScript>().BaldiSetDestination(hit.collider.gameObject);
                }

                //Detect Item Collection
                if (hit.collider.tag == "ItemTag" && hit.distance < 8.0f)
                {
                    if (hit.collider.gameObject.GetComponent<ItemScript>().idofpickup < 99) 
                    {
                        // Item collection logic -------------------------------------------
                        CheckInventoryPutObject(hit.collider.gameObject);
                    }
                    else
                    {
                        // Notebook logic --------------------------------------------------
                        GameController.GetComponent<GameControllerScript>().NotebookCount += 1;

                        if (sprint < 100f)
                        {
                            sprint = 100f;
                        }
                    
                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }
        //-------------------------------------------------------------------

        // Item Uses --------------------------------------------------------------
        if (Input.GetMouseButtonDown(1) && caught == false)
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
                
                //Principal's Key
                if (hit.collider.tag == "BlueDoor" && hit.distance < 8.0f && hit.collider.gameObject.transform.parent.gameObject.GetComponent<bluedoorscript>().lockedDoor == true && checkifItem(4))
                {
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<bluedoorscript>().unlockDoor();
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

                //Principal Behavior - To be moved to Principal ---------------------------------------
                Principal.GetComponent<PrincipalScript>().checkifseeBSODA();
                //-------------------------------------------------------------------------------------
                removeItem(selecteditem);
            }
        }

        // Inventory Selection ----------------------------------------------------
        if (caught == false)
        {
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

            if (Input.mouseScrollDelta.y == 1f)
            {
                selecteditem -= 1;

                if (selecteditem < 0) 
                {
                    selecteditem = 2;
                }
            }
            else if (Input.mouseScrollDelta.y == -1f)
            {
                selecteditem += 1;

                if (selecteditem > 2)
                {
                    selecteditem = 0;
                }
            }
        }
        //------------------------------------------------------------------------

        // UI Item Slot Updating ------- Update this 2 whenever you add new Items ------
        if (CheckIfLevel(SceneManager.GetActiveScene().name))
        {
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
                    case 4:
                    inventoryslots[i].SetActive(true);
                    inventoryslots[i].GetComponent<Image>().sprite = KeySprite;
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
        }
        //------------------------------------------------------------------------------

        // Camera flipping -------------------------------------------------------------
        if (caught == false)
        {
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
        }
        //------------------------------------------------------------------------------

        // Detention Time ----------------------------- Rest in Fixed Update -----------
        if (CheckIfLevel(SceneManager.GetActiveScene().name))
        {
            if (JailTime < 0f)
            {
                JailTime = 0f;
            }

            if (JailTime <= 0f)
            {
                PrincipalDoor.GetComponent<bluedoorscript>().unlockDoor();
                DetentionText.SetActive(false);
            }
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
        else if (checkifItem(4))
        {
            ItemText.GetComponent<Text>().text = "Principals Keys";
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
    public bool checkifItem(int i)
    {
        if (inventory[selecteditem] == i)
        {
            return true;
        }

        return false;
    }
    //------------------------------------------------------------------------

    // Protocol to put Items in someone's inventory ---------------------------
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
                // Also to be updated when a new item is added -----
                case 1:
                Instantiate(GameController.GetComponent<GameControllerScript>().YellowLock, item.transform.position, item.transform.rotation);
                break;

                case 2:
                Instantiate(GameController.GetComponent<GameControllerScript>().Zesty, item.transform.position, item.transform.rotation);
                break;

                case 3:
                Instantiate(GameController.GetComponent<GameControllerScript>().BSODA, item.transform.position, item.transform.rotation);
                break;

                case 4:
                Instantiate(GameController.GetComponent<GameControllerScript>().Key, item.transform.position, item.transform.rotation);
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
            Baldi.GetComponent<BaldiScript>().BaldiSetDestination(other.gameObject);
        }
        //-------------------------------------------------------------------

        // Entering Faculty Rooms -------------------------------------------
        if (other.tag == "FacultyTrigger")
        {
            insideofFaculty = true;
        }
        //-------------------------------------------------------------------

        // Office Room ------------------------------------------------------
        if (other.tag == "OfficeTrigger")
        {
            inJailTrigger = true;
        }
        //-------------------------------------------------------------------

        // Finish Doors -----------------------------------------------------
        if (other.tag == "FinalSwingDoor" && GameController.GetComponent<GameControllerScript>().NotebookCount == GameController.GetComponent<GameControllerScript>().NotebookTotal)
        {
            other.GetComponent<swingdoorscript>().open();
            other.GetComponent<swingdoorscript>().somethinginside = true;
            Baldi.GetComponent<BaldiScript>().BaldiSetDestination(other.gameObject);
            SceneManager.LoadScene("FinishScreen");
        }
        //-------------------------------------------------------------------

        // Start Door -------------------------------------------------------
        if (other.tag == "StartGameDoor")
        {
            other.GetComponent<swingdoorscript>().open();
            other.GetComponent<swingdoorscript>().somethinginside = true;
            SceneManager.LoadScene("SchoolHouse");
        }
        //-------------------------------------------------------------------
    }

    void OnCollisionEnter(Collision other)
    {
        // Rest of Bully Logic ----------------------------------------------
        if (other.gameObject.tag == "BullyCollider")
        {
            if (inventory[0] < 1 && inventory[1] < 1 && inventory[2] < 1)
            {
                Debug.Log("What, no items? No items, no paaassssss");
            }
            else
            {
                int SlotToSteal = Random.Range(0, 3);

                if (inventory[SlotToSteal] < 1)
                {
                    SlotToSteal += 1;
                    
                    if (SlotToSteal > 2) { SlotToSteal = 0; }

                    if (inventory[SlotToSteal] < 1)
                    {
                        SlotToSteal += 1;

                        if (SlotToSteal > 2) { SlotToSteal = 0; }
                    }
                }

                inventory[SlotToSteal] = 0;
                Debug.Log( "Thank you for your generous donationnnnn");

                //Temporary dissapearance, will replace with a proper method later
                Destroy(other.gameObject.transform.parent.gameObject);
            }
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
        if (other.tag == "FacultyTrigger")
        {
            insideofFaculty = false;
        }
        //-------------------------------------------------------------------

        // Office Room ------------------------------------------------------
        if (other.tag == "OfficeTrigger")
        {
            inJailTrigger = false;
        }
        //-------------------------------------------------------------------
    }

    public bool CheckIfLevel(string SceneName)
    {
        if (SceneName == "SchoolHouse")
        {
            return true;
        }

        return false;
    }
}
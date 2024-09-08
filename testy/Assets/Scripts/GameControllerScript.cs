using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    // Items --------------
    public GameObject BSODA;
    public GameObject Zesty;
    public GameObject YellowLock;
    public GameObject Key;
    public GameObject Tape;
    // ---------------------

    // Characters + Player -----
    public GameObject Principal;
    public GameObject Baldi;
    public GameObject Player;
    // -------------------------

    // Others ------------------
    public GameObject AIWanderPoints;
    public int NotebookCount;
    public int NotebookTotal = 1;
    public Text NotebookCountText;
    // -------------------------

    void Update()
    {
        NotebookCountText.GetComponent<Text>().text = NotebookCount + "/" + NotebookTotal + " Notebooks";

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public GameObject chooseWanderPoint()
    {
        int childrencount = 9;
        int pointtoChoose = Random.Range(0, childrencount);
        return AIWanderPoints.transform.GetChild(pointtoChoose).gameObject;
    }
}

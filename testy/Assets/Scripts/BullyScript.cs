using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullyScript : MonoBehaviour
{
    // TODO: Sounds
    // Rest of it is in PlayerScript.
    GameObject Player;
    public GameObject AIWanderPoints;
    private GameObject ChosenPoint;
    public Vector3 InitialSpawnPos;
    public bool SaidLine;
    public AudioSource AudioSource;
    public AudioClip SomethinGreat;
    public AudioClip GenerousDonation;
    public AudioClip NoItems;
    public GameObject GameController;

    void Start()
    {
        BullyTimer();

        // Game Objects as defined in Game Controller --------------------------------
        Player = GameController.GetComponent<GameControllerScript>().Player;
        // ---------------------------------------------------------------------------
    }

    public void BullyTimer()
    {
        float timing = Random.Range(20f, 50f);
        SaidLine = false;
        Invoke("SpawnBully", timing);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && SaidLine == false)
        {
            AudioSource.PlayOneShot(SomethinGreat);
            SaidLine = true;
        }
    }

    public void SpawnBully()
    {
        int SpawnPoint = Random.Range(0, AIWanderPoints.transform.childCount);

        for (int i = SpawnPoint; i < AIWanderPoints.transform.childCount + 1; i++)
        {
            if (AIWanderPoints.transform.GetChild(i).gameObject.tag == "HallPoint")
            {
                ChosenPoint = AIWanderPoints.transform.GetChild(i).gameObject;
                break;
            }

            if (SpawnPoint == AIWanderPoints.transform.childCount)
            {
                for (int j = 0; j < AIWanderPoints.transform.childCount; j++)
                {
                    if (AIWanderPoints.transform.GetChild(j).gameObject.tag == "HallPoint")
                    {
                        ChosenPoint = AIWanderPoints.transform.GetChild(j).gameObject;
                        break;
                    }
                }
            }
        }

        Vector3 SpawnPos = ChosenPoint.transform.position;

        // Y position of the Bully -----------------
        SpawnPos.y = 4.49f;
        //------------------------------------------

        this.gameObject.transform.position = SpawnPos;
    }

    public void OnBullyGone()
    {
        this.gameObject.transform.position = InitialSpawnPos;
        BullyTimer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullyScript : MonoBehaviour
{
    // TODO: Sounds
    // Rest of it is in PlayerScript.
    public GameObject Player;
    public GameObject AIWanderPoints;
    private GameObject ChosenPoint;

    void Start()
    {
        // TODO: Make this timed
        //SpawnBully();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Give me something greeeeeeeeeat");
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    // TODO: move a bunch of shit here
    public GameObject BSODA;
    public GameObject Zesty;
    public GameObject YellowLock;
    public GameObject AIWanderPoints;

    void Update()
    {
        
    }

    public GameObject chooseWanderPoint()
    {
        int childrencount = 9;
        int pointtoChoose = Random.Range(0, childrencount);
        return AIWanderPoints.transform.GetChild(pointtoChoose).gameObject;
    }
}

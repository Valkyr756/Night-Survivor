using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathTrigger : MonoBehaviour
{
    public GameObject player;

    public static bool isPlayerInBath;  //variable global para utilizarse en WaypointPatrol

    void Start()
    {
        isPlayerInBath = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
           isPlayerInBath = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInBath = false;
        }
    }
}

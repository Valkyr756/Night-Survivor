using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointZombie : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;
    public Transform[] distancias;

    int m_CurrentWaypointIndex;
    float dist;

    void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
        dist = Vector3.Distance(distancias[0].position, distancias[1].position);
    }

    void Update()
    {
        dist = Vector3.Distance(distancias[0].position, distancias[1].position);
        if(dist < 10)
            navMeshAgent.SetDestination(waypoints[1].position);
        /*if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }*/
    }
}

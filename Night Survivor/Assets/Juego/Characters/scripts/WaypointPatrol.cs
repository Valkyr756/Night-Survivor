using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform[] waypoints;

    int m_CurrentWaypointIndex;

    void Start()
    {

        navMeshAgent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        if(!BathTrigger.isPlayerInBath)                             //Si el jugador entra al baño el waypoint cambiará del jugador al waypoint 3
            navMeshAgent.SetDestination(waypoints[0].position);
        else
            navMeshAgent.SetDestination(waypoints[1].position);

        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Behavior_Manager : MonoBehaviour
{
    public float enabledAgentDistance = 10;
    private NavMeshAgent[] agents;

    private void Update()
    {
        UpdateAgentsList();
        EnableAgentsByDistance();
    }

    // Update the agents list
    public void UpdateAgentsList()
    {
        agents = Resources.FindObjectsOfTypeAll<NavMeshAgent>();
    }

    // Enable agents by distance with the player
    public void EnableAgentsByDistance()
    {
        foreach (NavMeshAgent agent in agents)
            agent.gameObject.SetActive(Vector3.Distance(agent.transform.position, transform.position) <= enabledAgentDistance);
    }
}

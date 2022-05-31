using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField]
    NavMeshSurface[] navMeshSurfaces;
    NavMeshAgent[] navMeshAgents;

    public void GetNavMeshSurfaces()
    {
        navMeshSurfaces = FindObjectsOfType<NavMeshSurface>();
    }

    public void Bake()
    {
        for (int i = 0; i < navMeshSurfaces.Length; i++)
            navMeshSurfaces[i].BuildNavMesh();

        // Activate all nav mesh agent after nav mesh baking
        navMeshAgents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent nma in navMeshAgents)
            nma.enabled = true;
    }
}

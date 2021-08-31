using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }
}

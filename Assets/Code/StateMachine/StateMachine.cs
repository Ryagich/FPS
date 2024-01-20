using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine : MonoBehaviour
{
    public float ViewRadius;
    [Range(0, 360)] public float ViewAngle;
    public float NearRadius; // Radius of NPC near area.
    public NavMeshAgent nav; // Reference to the NPC NavMesh agent.
    public List<Transform> PatrolPoints;

    private Patrol patrol;
    
    private void Awake()
    {
        
    }
}

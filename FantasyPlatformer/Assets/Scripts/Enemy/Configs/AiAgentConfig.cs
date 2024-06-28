using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    [Header("ChasePlayer")]
    public float chaseSpeed = 8f;
    public float maxChaseSpeed = 3f;

    [Header("Patrol")]
    public float patrolSpeed = 5f;
    public float maxPatrolSpeed = 3f;
    public float patrolWaitTime = 3.0f;
    public float patrolRadius = 10f;
    public float nextWaypointDistance = 3f;
    public float patrolZoneMinX = 0f;
    public float patrolZoneMaxX = 30f;
    public bool showPatrolZone = false;

    [Header("Attack")]
    public float attackForce = 20f;
    public float attackDistance = 2f;
    public float attackDuration = 0.6f;
    public float attackInterval = 0.3f;

    [Header("Stunned")]
    public float stunTime = 1f;
}

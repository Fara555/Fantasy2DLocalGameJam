using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
	[Header("ChasePlayer")]
	public float chaseSpeed = 2300f;
	public float maxChaseSpeed = 12f;

	[Header("Patrol")]
	public float patrolSpeed = 2000f;
	public float maxPatrolSpeed = 8f;
	public float patrolWaitTime = 3.0f;
	public float patrolRadius = 10f;
	public float nextWaypointDistance = 1f;
	public float patrolZoneMinX = 0f;
	public float patrolZoneMaxX = 45f;
	public bool showPatrolZone = false;

	[Header("Attack")]
	public float attackForce = 20f;
	public float attackDistance = 3f;
	public float attackDuration = 0.6f;
	public float attackInterval = 0.3f;
	public float dealDamageDistance = 3f;

	[Header("Stunned")]
	public float stunTime = 1f;
}

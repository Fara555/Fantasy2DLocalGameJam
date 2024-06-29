using System.Collections;
using UnityEngine;

public class BasePatrolState : AiState
{
	private bool move;
	private float lastGeneratedX;

	public float minDistanceBetweenPoints = 15f; // Set your minimum distance here

	public void Enter(AiAgent agent)
	{
		Debug.Log("base start patroling");
		lastGeneratedX = Random.Range(agent.initialX + agent.config.patrolZoneMinX, agent.initialX + agent.config.patrolZoneMaxX);
		agent.targetPoint = GeneratePoint(agent);
		agent.InvokeRepeating("UpdatePathToPoint", 0f, 0.3f); //start repeating "UpdatePath" method
	}

	public void Exit(AiAgent agent)
	{
		agent.CancelInvoke("UpdatePathToPoint"); //Stop repeat "UpdatePathToPoint" method
	}

	public AiStateId GetId()
	{
		return AiStateId.Patrol;
	}

	public void Update(AiAgent agent)
	{
		if (agent.sensor.seenPlayer)
		{
			agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
		}

		if (agent.path == null) return;

		if (agent.currentWaypoint >= agent.path.vectorPath.Count)
		{
			agent.reachedEndOfPath = true;
			return;
		}
		else agent.reachedEndOfPath = false;

		Vector2 direction = ((Vector2)agent.path.vectorPath[agent.currentWaypoint] - agent.rb.position).normalized;
		Vector2 force = direction * agent.config.patrolSpeed * Time.deltaTime;
		float distance = Vector2.Distance(agent.rb.position, agent.path.vectorPath[agent.currentWaypoint]);
		float pastDistance = Vector2.Distance(agent.rb.position, agent.targetPoint);

		// Apply force to move the agent
		if (move) agent.rb.AddForce(force);

		agent.animator.SetFloat("Speed", Mathf.Abs(agent.rb.velocity.x));

		// Limit the overall velocity to max speed
		if (agent.rb.velocity.magnitude > agent.config.maxPatrolSpeed) agent.rb.velocity = agent.rb.velocity.normalized * agent.config.maxPatrolSpeed;

		// Limit the x velocity component to prevent speeding up downhill
		if (Mathf.Abs(agent.rb.velocity.x) > agent.config.maxPatrolSpeed) agent.rb.velocity = new Vector2(Mathf.Sign(agent.rb.velocity.x) * agent.config.maxPatrolSpeed, agent.rb.velocity.y);

		if (distance < agent.config.nextWaypointDistance) agent.currentWaypoint++;

		if (pastDistance < agent.config.nextWaypointDistance)
		{
			move = false;
			// Stop the agent when it reaches the patrol point
			agent.rb.velocity = Vector2.zero;
			agent.rb.angularVelocity = 0f;

			// Wait before moving to the next waypoint
			if (!agent.once)
			{
				agent.once = true;
				agent.StartCoroutine(WaitForNextWaypoint(agent));
			}
		}
		else
		{
			move = true;
		}

		// Flip the sprite based on movement direction
		if (agent.rb.velocity.x > 0)
		{
			agent.transform.localScale = new Vector3(Mathf.Abs(agent.transform.localScale.x), agent.transform.localScale.y, agent.transform.localScale.z);
		}
		else if (agent.rb.velocity.x < 0)
		{
			agent.transform.localScale = new Vector3(-Mathf.Abs(agent.transform.localScale.x), agent.transform.localScale.y, agent.transform.localScale.z);
		}
	}

	private IEnumerator WaitForNextWaypoint(AiAgent agent)
	{
		yield return new WaitForSeconds(agent.config.patrolWaitTime);
		agent.targetPoint = GeneratePoint(agent);
		agent.once = false;
	}

	private Vector2 GeneratePoint(AiAgent agent)
	{
		float newX;
		do
		{
			newX = Random.Range(agent.initialX + agent.config.patrolZoneMinX, agent.initialX + agent.config.patrolZoneMaxX);
		} while (Mathf.Abs(newX - lastGeneratedX) < minDistanceBetweenPoints);

		lastGeneratedX = newX;

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(newX, agent.patrolCenter.y), Vector2.down, Mathf.Infinity, agent.groundLayers);

		if (hit.collider != null)
		{
			return hit.point + new Vector2(0, 1f); // Возвращаем координату, где рэйкаст столкнулся с землей
		}
		else
		{
			Debug.LogError("Ground not found below the generated coordinate.");
			return Vector2.zero; // Возвращаем нулевую координату в случае ошибки
		}
	}
}

using System.Collections;
using UnityEngine;

public class AiPatrolState : AiState
{
    private float requiredPastDistance = 3f;


    public void Enter(AiAgent agent)
    {
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
        if (agent.path == null)
        {
            return;
        }

        if (agent.currentWaypoint >= agent.path.vectorPath.Count)
        {
            agent.reachedEndOfPath = true;
            return;
        }
        else
        {
            agent.reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)agent.path.vectorPath[agent.currentWaypoint] - agent.rb.position).normalized;
        Vector2 force = direction * agent.config.patrolSpeed * Time.deltaTime;
        float distance = Vector2.Distance(agent.rb.position, agent.path.vectorPath[agent.currentWaypoint]);
        float pastDistance = Vector2.Distance(agent.rb.position, agent.patrolPoints[agent.currentPointIndex].position);

        // Apply force to move the agent
        agent.rb.AddForce(force);

        // Limit the overall velocity to max speed
        if (agent.rb.velocity.magnitude > agent.config.maxSpeed)
        {
            agent.rb.velocity = agent.rb.velocity.normalized * agent.config.maxSpeed;
        }

        // Limit the x velocity component to prevent speeding up downhill
        if (Mathf.Abs(agent.rb.velocity.x) > agent.config.maxSpeed)
        {
            agent.rb.velocity = new Vector2(Mathf.Sign(agent.rb.velocity.x) * agent.config.maxSpeed, agent.rb.velocity.y);
        }

        if (distance < agent.config.nextWaypointDistance)
        {
            agent.currentWaypoint++;
        }

        if (pastDistance < agent.config.nextWaypointDistance)
        {
            // Stop the agent when it reaches the patrol point
            agent.rb.velocity = Vector2.zero;
            agent.walk = false; // set animation

            // Wait before moving to the next waypoint
            if (!agent.once)
            {
                agent.once = true;
                agent.StartCoroutine(WaitForNextWaypoint(agent));
            }
        }
        else
        {
            agent.walk = true; // set animation
        }
    }

    private IEnumerator WaitForNextWaypoint(AiAgent agent)
    {
        yield return new WaitForSeconds(agent.config.patrolWaitTime);
        if (agent.currentPointIndex + 1 < agent.patrolPoints.Length)
        {
            agent.currentPointIndex++;
        }
        else
        {
            agent.currentPointIndex = 0;
        }
        agent.once = false;
    }
}

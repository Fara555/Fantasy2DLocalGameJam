using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChaseState : AiState
{
    public void Enter(AiAgent agent)
    {
        agent.InvokeRepeating("UpdatePath", 0f, 0.3f); //start repeating "UpdatePath" method
    }

    public void Exit(AiAgent agent)
    {
        agent.CancelInvoke("UpdatePath"); //Stop repeat "UpdatePathToPoint" method
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        CheckForAttackDistance(agent);

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
        Vector2 force = direction * agent.config.chaseSpeed * Time.deltaTime;
        float distanceToVectorPath = Vector2.Distance(agent.rb.position, agent.path.vectorPath[agent.currentWaypoint]);

        // Apply force to move the agent
        agent.rb.AddForce(force);
        agent.animator.SetFloat("Speed", Mathf.Abs(agent.rb.velocity.x));

        // Limit the overall velocity to max speed
        if (agent.rb.velocity.magnitude > agent.config.maxPatrolSpeed)
        {
            agent.rb.velocity = agent.rb.velocity.normalized * agent.config.maxPatrolSpeed;
        }



        // Limit the x velocity component to prevent speeding up downhill
        if (Mathf.Abs(agent.rb.velocity.x) > agent.config.maxPatrolSpeed)
        {
            agent.rb.velocity = new Vector2(Mathf.Sign(agent.rb.velocity.x) * agent.config.maxPatrolSpeed, agent.rb.velocity.y);
        }

        if (distanceToVectorPath < agent.config.nextWaypointDistance)
        {
            agent.currentWaypoint++;
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

    private void CheckForAttackDistance(AiAgent agent)
    {
        float distance = Vector2.Distance(agent.transform.position, agent.playerTransform.position);

        if (distance < agent.config.attackDistance)
        {
            agent.stateMachine.ChangeState(AiStateId.Attack);
        }
    }
}


using System.Collections;
using UnityEngine;

public class AiPatrolState : AiState
{
    private bool move;
    private Vector2 patrolAreaMin;
    private Vector2 patrolAreaMax;

    public void Enter(AiAgent agent)
    {
        // ”становите границы зоны патрулировани€ (например, по углам пр€моугольника)
        patrolAreaMin = new Vector2(-20, 10); // минимальные координаты
        patrolAreaMax = new Vector2(20, 2); // максимальные координаты

        SetRandomPatrolPoint(agent);

        agent.InvokeRepeating("UpdatePathToPoint", 0f, 0.3f); // start repeating "UpdatePath" method
    }

    public void Exit(AiAgent agent)
    {
        agent.CancelInvoke("UpdatePathToPoint"); // Stop repeat "UpdatePathToPoint" method
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

        // Apply force to move the agent
        if (move) agent.rb.AddForce(force);

        agent.animator.SetFloat("Speed", Mathf.Abs(agent.rb.velocity.x));

        // Limit the overall velocity to max speed
        if (agent.rb.velocity.magnitude > agent.config.maxPatrolSpeed)
            agent.rb.velocity = agent.rb.velocity.normalized * agent.config.maxPatrolSpeed;

        // Limit the x velocity component to prevent speeding up downhill
        if (Mathf.Abs(agent.rb.velocity.x) > agent.config.maxPatrolSpeed)
            agent.rb.velocity = new Vector2(Mathf.Sign(agent.rb.velocity.x) * agent.config.maxPatrolSpeed, agent.rb.velocity.y);

        if (distance < agent.config.nextWaypointDistance) agent.currentWaypoint++;

        if (agent.reachedEndOfPath)
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
        SetRandomPatrolPoint(agent);
        agent.once = false;
    }

    private void SetRandomPatrolPoint(AiAgent agent)
    {
        Vector2 randomPoint = GetRandomPointInPatrolArea(agent);
        agent.targetPoint = randomPoint;
    }

    private Vector2 GetRandomPointInPatrolArea(AiAgent agent)
    {
        Vector2 randomPoint;
        do
        {
            float randomX = Random.Range(patrolAreaMin.x, patrolAreaMax.x);
            float randomY = Random.Range(patrolAreaMin.y, patrolAreaMax.y);
            randomPoint = new Vector2(randomX, randomY);
        } while (!IsPointOnGround(randomPoint, agent));

        return randomPoint;
    }

    private bool IsPointOnGround(Vector2 point, AiAgent agent)
    {
        // ѕровер€ем, находитс€ ли точка на земле с помощью Physics2D.OverlapCircle
        Collider2D groundCollider = Physics2D.OverlapCircle(point, 0.1f, agent.groundLayers);
        return groundCollider != null;
    }
}

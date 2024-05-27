
using UnityEngine;

public class BasicEnemy: AiAgent
{
    protected override void Start()
    {
        base.Start();
    }
    public void UpdatePath()
    {
        if (agentSeeker.IsDone())
        {
            agentSeeker.StartPath(rb.position, playerTransform.position, OnPathComplete);
        }
    }

    public void UpdatePathToPoint()
    {
        if (agentSeeker.IsDone())
        {
            agentSeeker.StartPath(rb.position, targetPoint, OnPathComplete);
        }
    }

    public void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPoint, 5f);
    }
}
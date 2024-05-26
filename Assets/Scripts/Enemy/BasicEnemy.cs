using System.Collections;
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
            agentSeeker.StartPath(rb.position, patrolPoints[currentPointIndex].position, OnPathComplete);
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

}

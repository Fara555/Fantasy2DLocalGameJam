using UnityEngine;

public class BasicEnemy : AiAgent
{
    protected override void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        initialX = transform.position.x;
    }

    private void OnDrawGizmos()
    {
        if (config.showPatrolZone)
        {
            // ���������� ���� Gizmos
            Gizmos.color = Color.red;

            // ���������� ������ � ������� ����� ��� �������/��������
            Vector3 bottomLeft = new Vector3(initialX + config.patrolZoneMinX, transform.position.y - 3, 0); // ����������� ���������� �������� ��� y
            Vector3 topLeft = new Vector3(initialX + config.patrolZoneMinX, transform.position.y + 3, 0);    // ����������� ���������� �������� ��� y
            Vector3 bottomRight = new Vector3(initialX + config.patrolZoneMaxX, transform.position.y - 3, 0); // ����������� ���������� �������� ��� y
            Vector3 topRight = new Vector3(initialX + config.patrolZoneMaxX, transform.position.y + 3, 0);    // ����������� ���������� �������� ��� y

            // ��������� ����� ��� ��������
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}

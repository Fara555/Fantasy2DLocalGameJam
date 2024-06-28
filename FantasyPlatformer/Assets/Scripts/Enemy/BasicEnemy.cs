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
            // Установите цвет Gizmos
            Gizmos.color = Color.red;

            // Определите нижнюю и верхнюю точки для полоски/квадрата
            Vector3 bottomLeft = new Vector3(initialX + config.patrolZoneMinX, transform.position.y - 3, 0); // Используйте подходящее значение для y
            Vector3 topLeft = new Vector3(initialX + config.patrolZoneMinX, transform.position.y + 3, 0);    // Используйте подходящее значение для y
            Vector3 bottomRight = new Vector3(initialX + config.patrolZoneMaxX, transform.position.y - 3, 0); // Используйте подходящее значение для y
            Vector3 topRight = new Vector3(initialX + config.patrolZoneMaxX, transform.position.y + 3, 0);    // Используйте подходящее значение для y

            // Отрисовка линии или квадрата
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}

using UnityEngine;

public class SpearEnemy : AiAgent
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void RegisterStates()
	{
		stateMachine.RegisterState(new SpearChase());
		stateMachine.RegisterState(new SpearPatrol());
		stateMachine.RegisterState(new SpearAttack());
		stateMachine.RegisterState(new AiAFK());
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

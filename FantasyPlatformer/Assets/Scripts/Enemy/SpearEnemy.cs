using UnityEngine;

public class SpearEnemy : AiAgent
{
	[Header("Attack variables")]
	public float chargeDuration;

	protected override void Start()
	{
		base.Start();
	}

	protected override void RegisterStates()
	{
		stateMachine.RegisterState(new ChaseState());
		stateMachine.RegisterState(new PatrolState());
		stateMachine.RegisterState(new SpearAttack());
		stateMachine.RegisterState(new AiAFK());
	}

	private void OnEnable()
	{
		initialX = transform.position.x;
	}

	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}
}

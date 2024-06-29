using System.Collections;

public class BasemanAttack : AttackState, AiState
{
	public void Enter(AiAgent agent)
	{
		agent.StartCoroutine(MeleeAttack(agent));
	}

	public void Exit(AiAgent agent)
	{
		agent.StopCoroutine(MeleeAttack(agent));
	}

	public AiStateId GetId()
	{
		return AiStateId.Attack;
	}

	public void Update(AiAgent agent)
	{

	}

	public override IEnumerator MeleeAttack(AiAgent agent)
	{
		return base.MeleeAttack(agent);
	}
}

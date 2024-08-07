using System.Collections;
using UnityEngine;

public class SpearAttack : AttackState, AiState
{
	public void Enter(AiAgent agent)
	{
		Charge(agent);
	}

	public void Exit(AiAgent agent)
	{

	}

	public AiStateId GetId()
	{
		return AiStateId.Attack;
	}

	public void Update(AiAgent agent)
	{

	}

	public void Charge(AiAgent agent)
	{
		SpearEnemy spearEnemy = agent as SpearEnemy;

		spearEnemy.playerAttackPos = agent.playerTransform.position;
		agent.animator.Play("SpearmanCharge");
	}

	public override IEnumerator MeleeAttack(AiAgent agent)
	{
		SpearEnemy spearEnemy = agent as SpearEnemy;
		agent.animator.Play("AttackAnimation");

		Vector2 direction = (spearEnemy.playerAttackPos - (Vector2)agent.transform.position).normalized;
		Vector2 force = direction * agent.config.attackForce;
		agent.rb.AddForce(force, ForceMode2D.Impulse);

		float elapsedTime = 0f;
		while (elapsedTime < agent.config.attackDuration)
		{
			elapsedTime += Time.deltaTime;

			// Проверка расстояния до игрока
			float distance = Vector2.Distance(agent.transform.position, agent.playerTransform.position);
			if (distance < agent.config.attackDistance)
			{
				agent.playerHealth.DealDamage(20f);
				break;
			}
			yield return null;
		}
		yield return new WaitForSeconds(agent.config.attackInterval);

		agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
	}
}
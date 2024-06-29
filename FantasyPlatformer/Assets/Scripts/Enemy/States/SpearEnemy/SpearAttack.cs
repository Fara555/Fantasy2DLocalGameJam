using System.Collections;
using UnityEngine;

public class SpearAttack : AttackState, AiState
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
		SpearEnemy spearEnemy = agent as SpearEnemy;

		Vector2 initialPlayerPosition = agent.playerTransform.position;

		agent.animator.SetBool("Charge", true);

		yield return new WaitForSeconds(spearEnemy.chargeDuration);

		agent.animator.SetBool("Charge", false);
		agent.animator.SetBool("Attack", true);

		Vector2 direction = (initialPlayerPosition - (Vector2)agent.transform.position).normalized;
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

		// Остановка анимации атаки
		agent.animator.SetBool("Attack", false);

		// Задержка перед следующей атакой
		yield return new WaitForSeconds(agent.config.attackInterval);

		// Переход к состоянию преследования игрока
		agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
	}

}

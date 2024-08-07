using UnityEngine;

public class TestAnimFunctions : MonoBehaviour
{
	public SpearAttack attackState = new SpearAttack();
	private AiAgent agent;

	private void Start()
	{
		agent = GetComponentInParent<AiAgent>();
	}

	// Обёртка для вызова MeleeAttack
	public void CallMeleeAttack()
	{
		StartCoroutine(attackState.MeleeAttack(agent));
	}
}
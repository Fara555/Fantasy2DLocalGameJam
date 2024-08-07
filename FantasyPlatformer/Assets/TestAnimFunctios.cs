using UnityEngine;

public class TestAnimFunctions : MonoBehaviour
{
	public SpearAttack attackState = new SpearAttack();
	private AiAgent agent;

	private void Start()
	{
		agent = GetComponentInParent<AiAgent>();
	}

	// ������ ��� ������ MeleeAttack
	public void CallMeleeAttack()
	{
		StartCoroutine(attackState.MeleeAttack(agent));
	}
}
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class AiAttackState : AiState
{
    public void Enter(AiAgent agent)
    {
        agent.StartCoroutine(MeleeAtack(agent));
    }

    public void Exit(AiAgent agent)
    {
        agent.StopCoroutine(MeleeAtack(agent));
    }

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Update(AiAgent agent)
    {

    }

    private IEnumerator MeleeAtack(AiAgent agent)
    {
        agent.animator.SetBool("Attack", true);
        yield return new WaitForSeconds(agent.config.attackDuration);
        agent.animator.SetBool("Attack", false);

        float distance = Vector2.Distance(agent.transform.position, agent.playerTransform.position);
        if (distance < agent.config.attackDistance)
        {
            agent.playerHealth.DealDamage(20f);

            Vector2 direction = (agent.playerTransform.position - agent.transform.position).normalized;
            Vector2 force = direction * agent.config.attackForce;
            agent.playerRigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(agent.config.attackInterval);
        agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
    }
}
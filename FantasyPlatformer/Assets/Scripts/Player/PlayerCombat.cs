using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRate = 2f;

    [SerializeField] private bool drawAttackRange;

    [SerializeField] private float attackAnimationDuration;

    private float nextAttackTime = 0f;

    private void Start()
    {
        weaponSpriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("Attack"))
            {
                StartCoroutine(MeleeAtack());
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    private IEnumerator MeleeAtack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().DealDamage(attackDamage);
        }

        weaponSpriteRenderer.enabled = true;
        animator.SetBool("Atack", true);

        yield return new WaitForSeconds(attackAnimationDuration);

        weaponSpriteRenderer.enabled = false;
        animator.SetBool("Atack", false);
    }

    void OnDrawGizmos()
    {
        if (drawAttackRange)
        {
            if (attackPoint == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}

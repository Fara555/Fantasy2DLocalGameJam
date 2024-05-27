using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private float deathDelay;

    protected override void Update()
    {
        base.Update();

        if (currentHP <= 0)
        {
            StartCoroutine(EnemyDeath());
        }
    }

    protected override void Start()
    {
        base.Start();   
    }

    void OnEnable()
    {
        currentHP = maxHP;
    }

    private void OnDisable()
    {
        StopCoroutine(EnemyDeath());
    }

    public override void DealDamage(float damageAmount)
    {
        base.DealDamage(damageAmount);
    }

    private IEnumerator EnemyDeath()
    {
        Debug.Log("Enemy died!");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        yield return new WaitForSeconds(deathDelay);
        ObjectPoolManager.Instance.ReturnEnemyToPool(gameObject);
    }

    public void Revive(Vector3 position)
    {
        transform.position = position;
        GetComponent<Collider2D>().enabled = true;
        currentHP = maxHP;
    }

}

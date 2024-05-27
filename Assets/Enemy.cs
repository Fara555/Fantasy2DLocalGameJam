using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public float deathDelay = 2.0f; // Time in seconds before the enemy is deactivated

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            //die
        }
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        GetComponent<Collider2D>().enabled = true;
        this.enabled = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        // Any update logic if needed
    }
}

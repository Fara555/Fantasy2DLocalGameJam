using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float fireBallDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Fireball collided with: " + collision.gameObject.name);
        if (((1 << collision.gameObject.layer) & enemyMask) != 0)
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.DealDamage(fireBallDamage);
            }
            Debug.Log("Fireball hit an enemy and will deactivate");
            gameObject.SetActive(false);
        }
        else if (((1 << collision.gameObject.layer) & obstacleMask) != 0)
        {
            Debug.Log("Fireball hit an obstacle and will deactivate");
            gameObject.SetActive(false);
        }
    }

}

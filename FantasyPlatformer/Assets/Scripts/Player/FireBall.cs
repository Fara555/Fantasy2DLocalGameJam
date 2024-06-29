using UnityEngine;

public class FireBall : MonoBehaviour
{
	[SerializeField] private LayerMask enemyMask;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private float fireBallDamage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (((1 << collision.gameObject.layer) & enemyMask) != 0)
		{
			EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth != null)
			{
				enemyHealth.DealDamage(fireBallDamage);
			}
			gameObject.SetActive(false);
		}
		else if (((1 << collision.gameObject.layer) & obstacleMask) != 0)
		{
			gameObject.SetActive(false);
		}
	}

}

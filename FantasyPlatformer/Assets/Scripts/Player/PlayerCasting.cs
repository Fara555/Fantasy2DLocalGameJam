using System.Collections;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
	[SerializeField] private GameObject fireBall;
	[SerializeField] private CharacterController2D controller;
	[SerializeField] private Transform playerTransform;
	[SerializeField] private ObjectPoolManager fireBallPool;
	[SerializeField] private float fireBallSpeed;
	[SerializeField] private float fireBallLifeTime;
	[SerializeField] private float fireBallCooldown;

	private float lastFireTime;

	private void Update()
	{

		if (Input.GetButtonDown("CastSpell") && Time.time >= lastFireTime + fireBallCooldown)
		{
			ShootFireball();
			lastFireTime = Time.time;
		}
	}

	private void ShootFireball()
	{
		GameObject projectile = fireBallPool.GetPooledObject();
		Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
		projectile.transform.position = playerTransform.position;
		projectile.transform.rotation = Quaternion.identity;
		projectile.SetActive(true);

		rb.velocity = Vector2.zero;

		Vector2 force;
		if (controller.facingRight)
		{
			force = Vector2.right;
		}
		else
		{
			force = Vector2.left;
		}

		// Применяем силу к проджектайлу
		rb.AddForce(force * fireBallSpeed, ForceMode2D.Impulse);
		StartCoroutine(DisableSpellAfterTime(fireBallLifeTime, projectile));
	}

	private IEnumerator DisableSpellAfterTime(float time, GameObject spell)
	{
		yield return new WaitForSeconds(time);
		if (spell.activeSelf)
		{
			spell.SetActive(false);
		}
	}
}
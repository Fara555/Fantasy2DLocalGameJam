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
    [SerializeField] private float fireBallCooldown; // Продолжительность кулдауна в секундах

    private float lastFireTime; // Время последнего использования фаербола

    private void Update()
    {
        // Проверяем, можно ли кастовать фаербол
        if (Input.GetButtonDown("CastSpell") && Time.time >= lastFireTime + fireBallCooldown)
        {
            ShootFireball();
            lastFireTime = Time.time; // Обновляем время последнего использования фаербола
        }
    }

    private void ShootFireball()
    {
        GameObject projectile = fireBallPool.GetPooledObject();
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        projectile.transform.position = playerTransform.position;
        projectile.transform.rotation = Quaternion.identity;
        projectile.SetActive(true);

        // Сбрасываем скорость проджектайла перед применением новой силы
        rb.velocity = Vector2.zero;

        // Определяем направление в зависимости от направления взгляда игрока
        Vector2 force;
        if (controller.facingRight)
        {
            force = Vector2.right;  // Стрельба вправо
        }
        else
        {
            force = Vector2.left;  // Стрельба влево
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
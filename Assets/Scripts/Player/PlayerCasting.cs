using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] private GameObject fireBall;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private ObjectPoolManager fireBallPool;
    [SerializeField] private float fireBallSpeed;

    private void Update()
    {
        if (Input.GetButtonDown("CastSpell"))
        {
            ShootFireball();
        }
    }

    private void ShootFireball()
    {
        GameObject projectile = fireBallPool.GetPooledObject();
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        projectile.SetActive(true);
        projectile.transform.position = playerTransform.position;

        // ���������� ����������� � ����������� �� ����������� ������� ������
        Vector2 force;
        if (controller.facingRight)
        {
            force = Vector2.right;  // �������� ������
        }
        else
        {
            force = Vector2.left;  // �������� �����
        }

        // ��������� ���� � ������������
        rb.AddForce(force * fireBallSpeed, ForceMode2D.Impulse);
    }
}
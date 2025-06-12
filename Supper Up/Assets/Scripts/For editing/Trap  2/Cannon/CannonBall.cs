using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private Vector3 fireDirection;
    public float pushForce = 10f; // �÷��̾ �о�� ��

    public void Initialize(float bulletSpeed, float bulletLifetime, Vector3 dir)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        fireDirection = dir;

        GetComponent<Rigidbody>().AddForce(dir * speed, ForceMode.Impulse);

        Destroy(gameObject, lifetime);
    }

    //void Update()
    //{
    //    // �Ѿ��� ������ �̵�
    //    transform.Translate(fireDirection * speed * Time.deltaTime);
    //}

    void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �浹 �������� �÷��̾� ���������� ���� ���
                Vector3 pushDirection = fireDirection.normalized;

                // �÷��̾�� ���� ����
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.isTrusted = true;
            }
        }

        // �浹 �� �Ѿ� �ı�
        Destroy(gameObject);
    }
}
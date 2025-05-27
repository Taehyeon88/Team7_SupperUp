using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private float speed;
    private float lifetime;
    public float pushForce = 10f; // �÷��̾ �о�� ��

    public void Initialize(float bulletSpeed, float bulletLifetime)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // �Ѿ��� ������ �̵�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // �÷��̾�� �浹�ߴ��� Ȯ��
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // �浹 �������� �÷��̾� ���������� ���� ���
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

                // �÷��̾�� ���� ����
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }

        // �浹 �� �Ѿ� �ı�
        Destroy(gameObject);
    }
}
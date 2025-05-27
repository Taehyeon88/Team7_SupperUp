using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private float speed;
    private float lifetime;
    public float pushForce = 10f; // 플레이어를 밀어내는 힘

    public void Initialize(float bulletSpeed, float bulletLifetime)
    {
        speed = bulletSpeed;
        lifetime = bulletLifetime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 총알을 앞으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 플레이어와 충돌했는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 충돌 지점에서 플레이어 방향으로의 벡터 계산
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;

                // 플레이어에게 힘을 가함
                playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
            }
        }

        // 충돌 후 총알 파괴
        Destroy(gameObject);
    }
}
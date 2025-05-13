using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float pushForce = 500f; // 플레이어를 밀쳐내는 힘

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized; // 방향 계산
                playerRb.AddForce(pushDirection * pushForce); // 플레이어를 밀쳐내기
            }
        }
    }
}

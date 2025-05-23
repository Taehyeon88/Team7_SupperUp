using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float pushForce = 500f; // �÷��̾ ���ĳ��� ��

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized; // ���� ���
                playerRb.AddForce(pushDirection * pushForce); // �÷��̾ ���ĳ���
            }
        }
    }
}

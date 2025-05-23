using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    public float swingSpeed = 2f; // ���� ������ �ӵ�
    public float maxSwingAngle = 60f; // �ִ� ȸ�� ����
    public float knockbackForce = 10f; // �÷��̾� ���ĳ��� ��
    public float axeLength = 5f; // ������ ����

    private float currentAngle = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // �ð��� � ����
        currentAngle = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // ȸ�� �� ����
        Vector3 offset = transform.up * axeLength;
        transform.position = originalPosition - offset + transform.rotation * offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� ���� �� ��� ���ĳ���
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
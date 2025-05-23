using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : MonoBehaviour
{
    public float swingSpeed = 2f; // 도끼 움직임 속도
    public float maxSwingAngle = 60f; // 최대 회전 각도
    public float knockbackForce = 10f; // 플레이어 밀쳐내는 힘
    public float axeLength = 5f; // 도끼의 길이

    private float currentAngle = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        // 시계추 운동 구현
        currentAngle = maxSwingAngle * Mathf.Sin(Time.time * swingSpeed);
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // 회전 축 보정
        Vector3 offset = transform.up * axeLength;
        transform.position = originalPosition - offset + transform.rotation * offset;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 접촉 시 즉시 밀쳐내기
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 knockbackDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}
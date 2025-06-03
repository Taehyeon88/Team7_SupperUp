using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 1f; // �ʴ� �߻� Ƚ��
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    [Header("Detection Settings")]
    public float detectDistance = 30f;        // ���� �Ÿ�
    public float viewAngle = 45f;             // �þ߰� (�� ����)

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float nextFireTime;

    private Transform playerTransform; // �÷��̾� ��ġ ����

    void Start()
    {
        nextFireTime = 0f;
        // �÷��̾� ã��
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("PlayerController not found in scene.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            if (CanSeePlayer())
            {
                if (Time.time >= nextFireTime)
                {
                    Fire();
                    nextFireTime = Time.time + 1f / fireRate;
                }
            }
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distance = directionToPlayer.magnitude;

        // �Ÿ� üũ
        if (distance > detectDistance)
            return false;

        // ������ �þ߰� ���� �ִ��� üũ
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < viewAngle / 2)
        {
            // �÷��̾ �þ� ����, �׸��� �Ÿ� ���� �ִٸ�
            return true;
        }

        return false;
    }

    void Fire()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        CannonBall bullet = bulletObject.GetComponent<CannonBall>();
        if (bullet != null)
        {
            bullet.Initialize(bulletSpeed, bulletLifetime);
        }
    }
}

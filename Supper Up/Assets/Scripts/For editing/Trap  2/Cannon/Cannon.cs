using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 1f; // 초당 발사 횟수
    public float bulletSpeed = 20f;
    public float bulletLifetime = 3f;

    [Header("Detection Settings")]
    public float detectDistance = 30f;        // 감지 거리
    public float viewAngle = 45f;             // 시야각 (도 단위)

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float nextFireTime;

    private Transform playerTransform; // 플레이어 위치 참조

    void Start()
    {
        nextFireTime = 0f;
        // 플레이어 찾기
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

        // 거리 체크
        if (distance > detectDistance)
            return false;

        // 방향이 시야각 내에 있는지 체크
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < viewAngle / 2)
        {
            // 플레이어가 시야 내에, 그리고 거리 내에 있다면
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

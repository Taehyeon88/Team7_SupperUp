using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 1f; // 초당 발사 횟수
    public float bulletSpeed = 20f; // 총알 속도
    public float bulletLifetime = 3f; // 총알 수명

    [Header("References")]
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint; // 총알이 발사되는 위치

    private float nextFireTime;

    void Start()
    {
        nextFireTime = 0f;
    }

    void Update()
    {
        // 발사 시간이 되면 총알 발사
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        // 총알 생성
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Bullet 스크립트에 속도 전달
        CannonBall bullet = bulletObject.GetComponent<CannonBall>();
        if (bullet != null)
        {
            bullet.Initialize(bulletSpeed, bulletLifetime);
        }
    }
}
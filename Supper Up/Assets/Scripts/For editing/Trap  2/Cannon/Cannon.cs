using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Gun Settings")]
    public float fireRate = 1f; // �ʴ� �߻� Ƚ��
    public float bulletSpeed = 20f; // �Ѿ� �ӵ�
    public float bulletLifetime = 3f; // �Ѿ� ����

    [Header("References")]
    public GameObject bulletPrefab; // �Ѿ� ������
    public Transform firePoint; // �Ѿ��� �߻�Ǵ� ��ġ

    private float nextFireTime;

    void Start()
    {
        nextFireTime = 0f;
    }

    void Update()
    {
        // �߻� �ð��� �Ǹ� �Ѿ� �߻�
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        // �Ѿ� ����
        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Bullet ��ũ��Ʈ�� �ӵ� ����
        CannonBall bullet = bulletObject.GetComponent<CannonBall>();
        if (bullet != null)
        {
            bullet.Initialize(bulletSpeed, bulletLifetime);
        }
    }
}
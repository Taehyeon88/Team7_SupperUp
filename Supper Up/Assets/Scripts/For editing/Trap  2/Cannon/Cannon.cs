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
    public LayerMask playerLayer;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float nextFireTime;

    private Transform playerTransform; // �÷��̾� ��ġ ����
    private AudioSource playAudio;

    void Start()
    {
        playAudio = GetComponent<AudioSource>();
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
        //Vector3 directionToPlayer = playerTransform.position - transform.position;
        //float distance = directionToPlayer.magnitude;

        //// �Ÿ� üũ
        //if (distance > detectDistance)
        //    return false;

        //// ������ �þ߰� ���� �ִ��� üũ
        //float angle = Vector3.Angle(transform.forward, directionToPlayer);

        //if (angle < viewAngle / 2)
        //{
        //    // �÷��̾ �þ� ����, �׸��� �Ÿ� ���� �ִٸ�
        //    return true;
        //}

        //return false;

        if (Physics.SphereCast(firePoint.transform.position, 0.4f, transform.forward, out RaycastHit hit, 46.5f, playerLayer))
        {
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
            bullet.Initialize(bulletSpeed, bulletLifetime, transform.forward);
        }

        if (playAudio != null) playAudio.Play();
        else
        {
            playAudio = GetComponent<AudioSource>();
            playAudio.Play();
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(firePoint.transform.position, transform.forward * 46.5f, Color.red);
    }
}

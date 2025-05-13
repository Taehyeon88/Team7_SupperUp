using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab; // 대포에서 발사할 공 프리팹
    public float fireForce = 500f; // 발사 속도
    public float fireRate = 2f; // 발사 간격
    public float ballLifetime = 5f; // 공의 생존 시간
    public float cannonBallOffset = 1f; // 대포에서 대포알까지의 거리(offset)

    void Start()
    {
        StartCoroutine(FireCannon());
    }

    IEnumerator FireCannon()
    {
        while (true)
        {
            Fire();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Fire()
    {
        // 대포에서 공을 생성, 대포의 위치에 맞춰 조정
        Vector3 spawnPosition = transform.position + transform.forward * cannonBallOffset; // 대포알 위치 조정
        GameObject cannonBall = Instantiate(cannonBallPrefab, spawnPosition, transform.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 대포의 전방으로 힘을 주어 발사
            rb.AddForce(transform.forward * fireForce);
        }

        // 공의 생존 시간 설정
        Destroy(cannonBall, ballLifetime);
    }
}
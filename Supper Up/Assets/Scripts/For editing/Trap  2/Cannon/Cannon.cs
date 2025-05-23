using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject cannonBallPrefab; // �������� �߻��� �� ������
    public float fireForce = 500f; // �߻� �ӵ�
    public float fireRate = 2f; // �߻� ����
    public float ballLifetime = 5f; // ���� ���� �ð�
    public float cannonBallOffset = 1f; // �������� �����˱����� �Ÿ�(offset)

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
        // �������� ���� ����, ������ ��ġ�� ���� ����
        Vector3 spawnPosition = transform.position + transform.forward * cannonBallOffset; // ������ ��ġ ����
        GameObject cannonBall = Instantiate(cannonBallPrefab, spawnPosition, transform.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // ������ �������� ���� �־� �߻�
            rb.AddForce(transform.forward * fireForce);
        }

        // ���� ���� �ð� ����
        Destroy(cannonBall, ballLifetime);
    }
}
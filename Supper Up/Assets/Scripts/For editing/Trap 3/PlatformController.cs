using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private GameObject bridgePrefab;  // ������ �� ���� ������

    [SerializeField]
    private Vector3 bridgeOffset = new Vector3(1, 0, 0); // ���� ��ġ ������ ������

    private GameObject currentBridge; // ���� ������ ����
    private bool playerOnPlatform = false; // �÷��̾ ���� ���� �ִ��� ����
    private float timer = 0f; // �ð� ����
    [SerializeField]
    private float disappearTime = 5f;  // ������ ����� �ð� (��)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player collided with platform.");
            playerOnPlatform = true;
            timer = 0f;

            // ������ �̹� ������ ���� ����
            if (currentBridge == null)
            {
                Vector3 spawnPosition = transform.position + bridgeOffset;
                currentBridge = Instantiate(bridgePrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Bridge instantiated at: " + spawnPosition);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player separated from platform.");
            playerOnPlatform = false;
            timer = 0f; // �ð� ������ �ٽ� ����
        }
    }

    private void Update()
    {
        if (!playerOnPlatform && currentBridge != null)
        {
            timer += Time.deltaTime;
            if (timer >= disappearTime)
            {
                Destroy(currentBridge);
                currentBridge = null;
                Debug.Log("Bridge destroyed");
            }
        }
    }
}

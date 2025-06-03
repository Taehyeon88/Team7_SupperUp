using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private GameObject bridgePrefab;  // 생성할 새 발판 프리팹

    [SerializeField]
    private Vector3 bridgeOffset = new Vector3(1, 0, 0); // 발판 위치 조절용 오프셋

    private GameObject currentBridge; // 현재 생성된 발판
    private bool playerOnPlatform = false; // 플레이어가 발판 위에 있는지 여부
    private float timer = 0f; // 시간 계산용
    [SerializeField]
    private float disappearTime = 5f;  // 발판이 사라질 시간 (초)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Player collided with platform.");
            playerOnPlatform = true;
            timer = 0f;

            // 발판이 이미 없으면 새로 생성
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
            timer = 0f; // 시간 측정을 다시 시작
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

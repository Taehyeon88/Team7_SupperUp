using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCreator : MonoBehaviour
{   
    public GameObject bridgePrefab;                   // 생성할 다리 프리팹
    private float bridgeLifetime = 5f;                // 다리가 유지되는 시간 (초)
    private bool isPlayerOnPlatform = false;          // 플레이어가 발판 위에 있는지 여부

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 발판에 도착했을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            StartCoroutine(SpawnBridge());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 발판에서 떨어졌을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }

    private IEnumerator SpawnBridge()
    {
        // 발판 위에 플레이어가 있는 동안 다리 생성
        while (isPlayerOnPlatform)
        {
            GameObject bridge = Instantiate(bridgePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(bridgeLifetime);
            Destroy(bridge);
        }
    }
}

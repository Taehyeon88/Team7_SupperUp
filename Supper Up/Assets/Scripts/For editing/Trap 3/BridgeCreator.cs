using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCreator : MonoBehaviour
{   
    public GameObject bridgePrefab;                   // ������ �ٸ� ������
    private float bridgeLifetime = 5f;                // �ٸ��� �����Ǵ� �ð� (��)
    private bool isPlayerOnPlatform = false;          // �÷��̾ ���� ���� �ִ��� ����

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾ ���ǿ� �������� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            StartCoroutine(SpawnBridge());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // �÷��̾ ���ǿ��� �������� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }

    private IEnumerator SpawnBridge()
    {
        // ���� ���� �÷��̾ �ִ� ���� �ٸ� ����
        while (isPlayerOnPlatform)
        {
            GameObject bridge = Instantiate(bridgePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(bridgeLifetime);
            Destroy(bridge);
        }
    }
}

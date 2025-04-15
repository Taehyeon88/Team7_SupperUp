using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    private bool isFalling = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private System.Collections.IEnumerator Fall()
    {
        isFalling = true;

        // 다리가 떨어지기 전에 짧은 대기
        yield return new WaitForSeconds(1f);

        // 다리 물리 적용
        // 다리를 제거하거나 적절한 위치로 떨어뜨리는 코드 삽입
        // 이 경우 Rigidbody가 필요 없으므로 그냥 바로 제거해버리거나 다른 방식으로 처리
        Destroy(gameObject);
        // 또는 다리를 비활성화할 수 있습니다: gameObject.SetActive(false);
    }
}

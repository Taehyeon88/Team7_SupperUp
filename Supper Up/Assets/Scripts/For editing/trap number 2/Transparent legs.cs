using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    private bool isFalling = false;

    [Header("Timing Settings")]
    [SerializeField]
    private float disappearDelay = 2f; // 사라지는 시간 (유니티 인스펙터에서 조정 가능)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true; // 상태를 '떨어진'으로 변경

        // 사라지기 전의 대기 시간
        yield return new WaitForSeconds(disappearDelay);

        // 다리를 비활성화 (사라짐)
        gameObject.SetActive(false);

        // 다시 밟을 수 있도록 상태 초기화 (발판이 비활성화된 후 상태를 초기화합니다)
        isFalling = false;
    }
}


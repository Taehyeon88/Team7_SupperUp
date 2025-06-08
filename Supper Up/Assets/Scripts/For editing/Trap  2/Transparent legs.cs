using UnityEngine;
using System.Collections;

public class TransparentLegs : MonoBehaviour
{
    public float vanishDelay = 5.0f;      // 플레이어가 밟고 난 후 사라지는 시간 (초)
    public float reappearDelay = 5.0f;    // 사라졌다가 다시 나타나는 시간 (초)

    private bool isTriggered = false;     // 중복 Trigger 방지용

    private Renderer gimmick_Rend;          // 렌더러 컴포넌트 (보이기/숨기기)
    private Collider gimmick_Col;           // 콜라이더 컴포넌트 (충돌 감지)

    void Awake()
    {
        gimmick_Rend = GetComponent<Renderer>();
        gimmick_Col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 "Player"인 오브젝트와 충돌하면 시작
        if (!isTriggered && collision.transform.CompareTag("Player"))
        {
            isTriggered = true; // 다시 트리거 방지
            StartCoroutine(VanishAndReappear());
        }
    }

    private IEnumerator VanishAndReappear()
    {
        // 일정 시간 후 투명/비활성화
        yield return new WaitForSeconds(vanishDelay);

        gimmick_Rend.enabled = false;
        gimmick_Col.enabled = false;

        // 일정 시간 후 다시 활성화
        yield return new WaitForSeconds(reappearDelay);

        gimmick_Rend.enabled = true;
        gimmick_Col.enabled = true;

        isTriggered = false; // 다시 트리거 허용
    }
}
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField]
    private float fallDelay = 2f;        // 플레이어 접촉 후 사라지는 시간
    [SerializeField]
    private float respawnDelay = 5f;     // 사라졌다 다시 나타나는 시간

    private bool isTriggered = false;    // 중복 방지용

    // 원래 위치 저장 (필요시)
    private Vector3 originalPosition;
    public Collider[] colliders;
    public MeshRenderer[] renderers;
    public AudioClip playClip;           //부서지는 사운드 클립

    void Start()
    {
        originalPosition = transform.position;
    }

    // 충돌 감지
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private IEnumerator DestroyAndRespawn()
    {
        isTriggered = true;

        // 일정 시간 후에 다리 사라짐
        yield return new WaitForSeconds(fallDelay);

        //Debug.Log(respawnDelay);
        if (SoundManager.instance != null) SoundManager.instance.PlayAllSoundWithClip(playClip);

        for (int i = 0; i < renderers.Length; i++)
        {
            colliders[i].enabled = false;
            renderers[i].enabled = false;
        }

        // 일정 시간 후 다시 활성화 (다시 생김)
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log(respawnDelay);

        isTriggered = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            colliders[i].enabled = true;
            renderers[i].enabled = true;
        }
    }

    private void OnEnable()
    {
        // 오브젝트 재활성화 될 때 초기화
        isTriggered = false;
        // 필요시 위치, 색상 초기화 등 추가 가능
        // transform.position = originalPosition;  // 위치 초기화
    }
}

using System.Collections;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField]
    private float fallDelay = 2f;        // �÷��̾� ���� �� ������� �ð�
    [SerializeField]
    private float respawnDelay = 5f;     // ������� �ٽ� ��Ÿ���� �ð�

    private bool isTriggered = false;    // �ߺ� ������

    // ���� ��ġ ���� (�ʿ��)
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    // �浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private IEnumerator DestroyAndRespawn()
    {
        isTriggered = true;

        // ���� �ð� �Ŀ� �ٸ� �����
        yield return new WaitForSeconds(fallDelay);
        gameObject.SetActive(false); // ��Ȱ��ȭ - �����

        // ���� �ð� �� �ٽ� Ȱ��ȭ (�ٽ� ����)
        yield return new WaitForSeconds(respawnDelay);
        gameObject.SetActive(true); // �ٽ� Ȱ��ȭ (�ٽ� ��Ÿ��)
    }

    private void OnEnable()
    {
        // ������Ʈ ��Ȱ��ȭ �� �� �ʱ�ȭ
        isTriggered = false;
        // �ʿ�� ��ġ, ���� �ʱ�ȭ �� �߰� ����
        // transform.position = originalPosition;  // ��ġ �ʱ�ȭ
    }
}

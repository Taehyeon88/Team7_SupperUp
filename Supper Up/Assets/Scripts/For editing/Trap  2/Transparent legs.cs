using UnityEngine;
using System.Collections;

public class TransparentLegs : MonoBehaviour
{
    public float vanishDelay = 5.0f;      // �÷��̾ ��� �� �� ������� �ð� (��)
    public float reappearDelay = 5.0f;    // ������ٰ� �ٽ� ��Ÿ���� �ð� (��)

    private bool isTriggered = false;     // �ߺ� Trigger ������

    private Renderer gimmick_Rend;          // ������ ������Ʈ (���̱�/�����)
    private Collider gimmick_Col;           // �ݶ��̴� ������Ʈ (�浹 ����)

    void Awake()
    {
        gimmick_Rend = GetComponent<Renderer>();
        gimmick_Col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �±װ� "Player"�� ������Ʈ�� �浹�ϸ� ����
        if (!isTriggered && collision.transform.CompareTag("Player"))
        {
            isTriggered = true; // �ٽ� Ʈ���� ����
            StartCoroutine(VanishAndReappear());
        }
    }

    private IEnumerator VanishAndReappear()
    {
        // ���� �ð� �� ����/��Ȱ��ȭ
        yield return new WaitForSeconds(vanishDelay);

        gimmick_Rend.enabled = false;
        gimmick_Col.enabled = false;

        // ���� �ð� �� �ٽ� Ȱ��ȭ
        yield return new WaitForSeconds(reappearDelay);

        gimmick_Rend.enabled = true;
        gimmick_Col.enabled = true;

        isTriggered = false; // �ٽ� Ʈ���� ���
    }
}
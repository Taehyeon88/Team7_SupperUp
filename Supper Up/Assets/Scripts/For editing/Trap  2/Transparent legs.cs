using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    private bool isFalling = false;

    [Header("Timing Settings")]
    [SerializeField]
    private float disappearDelay = 2f; // ������� �ð� (����Ƽ �ν����Ϳ��� ���� ����)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true; // ���¸� '������'���� ����

        // ������� ���� ��� �ð�
        yield return new WaitForSeconds(disappearDelay);

        // �ٸ��� ��Ȱ��ȭ (�����)
        gameObject.SetActive(false);

        // �ٽ� ���� �� �ֵ��� ���� �ʱ�ȭ (������ ��Ȱ��ȭ�� �� ���¸� �ʱ�ȭ�մϴ�)
        isFalling = false;
    }
}


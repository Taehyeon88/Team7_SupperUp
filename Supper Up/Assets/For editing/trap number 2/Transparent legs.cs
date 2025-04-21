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

        // �ٸ��� �������� ���� ª�� ���
        yield return new WaitForSeconds(1f);

        // �ٸ� ���� ����
        // �ٸ��� �����ϰų� ������ ��ġ�� ����߸��� �ڵ� ����
        // �� ��� Rigidbody�� �ʿ� �����Ƿ� �׳� �ٷ� �����ع����ų� �ٸ� ������� ó��
        Destroy(gameObject);
        // �Ǵ� �ٸ��� ��Ȱ��ȭ�� �� �ֽ��ϴ�: gameObject.SetActive(false);
    }
}

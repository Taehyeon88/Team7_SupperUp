using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public List<GameObject> platforms; // ���� ������Ʈ���� �巡�� �� ����� ����Ʈ

    private void Start()
    {
        // ó������ ù ��° ���Ǹ� Ȱ��ȭ
        InitializePlatforms();
        StartCoroutine(PlatformCycle());
    }

    private void InitializePlatforms()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            if (i == 0) // ù ��° ���Ǹ� Ȱ��ȭ
            {
                ActivatePlatform(i);
            }
            else // �������� ��Ȱ��ȭ
            {
                DeactivatePlatform(i);
            }
        }
    }

    private IEnumerator PlatformCycle()
    {
        while (true)
        {
            // Ȧ�� �ε���(1, 3, 5...) Ȱ��ȭ�ϰ� ¦�� �ε���(0, 2, 4...) ��Ȱ��ȭ
            for (int i = 0; i < platforms.Count; i++)
            {
                if (i % 2 == 1) // Ȧ�� �ε����� ��
                {
                    ActivatePlatform(i);
                }
                else // ¦�� �ε����� ��
                {
                    DeactivatePlatform(i);
                }
            }

            yield return new WaitForSeconds(2f); // Ȧ�� �ε��� ���� Ȱ��ȭ ���� �ð�

            // ¦�� �ε���(0, 2, 4...) Ȱ��ȭ�ϰ� Ȧ�� �ε���(1, 3, 5...) ��Ȱ��ȭ
            for (int i = 0; i < platforms.Count; i++)
            {
                if (i % 2 == 0) // ¦�� �ε����� ��
                {
                    ActivatePlatform(i);
                }
                else // Ȧ�� �ε����� ��
                {
                    DeactivatePlatform(i);
                }
            }

            yield return new WaitForSeconds(2f); // ¦�� �ε��� ���� Ȱ��ȭ ���� �ð�
        }
    }

    private void ActivatePlatform(int index)
    {
        platforms[index].GetComponent<Renderer>().enabled = true;
        platforms[index].GetComponent<Collider>().enabled = true;
    }

    private void DeactivatePlatform(int index)
    {
        platforms[index].GetComponent<Renderer>().enabled = false;
        platforms[index].GetComponent<Collider>().enabled = false;
    }
}
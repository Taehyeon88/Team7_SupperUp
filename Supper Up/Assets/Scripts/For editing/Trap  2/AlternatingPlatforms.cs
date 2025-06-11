using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingPlatforms : MonoBehaviour
{
    public List<GameObject> platforms; // ���� ������Ʈ���� �巡�� �� ����� ����Ʈ
    public float platformCycleTime = 2f; // ���� ��ȭ �ֱ�
    private float currentSpeed = 1f;

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

            yield return new WaitForSeconds(platformCycleTime / currentSpeed); // Ȧ�� �ε��� ���� Ȱ��ȭ ���� �ð�

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

            yield return new WaitForSeconds(platformCycleTime / currentSpeed); // ¦�� �ε��� ���� Ȱ��ȭ ���� �ð�
        }
    }

    private void ActivatePlatform(int index)
    {
        platforms[index].GetComponent<Collider>().enabled = true;
        platforms[index].transform.GetChild(0).GetComponent<Renderer>().enabled = true;

    }

    private void DeactivatePlatform(int index)
    {
        platforms[index].GetComponent<Collider>().enabled = false;
        platforms[index].transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    public void ChangeSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, 0.1f, 5f); // �ӵ� ���� ����
        Debug.Log("Current Platform Speed: " + currentSpeed);
    }
}

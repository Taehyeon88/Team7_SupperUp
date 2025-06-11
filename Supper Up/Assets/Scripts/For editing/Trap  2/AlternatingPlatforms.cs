using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingPlatforms : MonoBehaviour
{
    public List<GameObject> platforms; // 발판 오브젝트들을 드래그 앤 드롭할 리스트
    public float platformCycleTime = 2f; // 발판 변화 주기
    private float currentSpeed = 1f;

    private void Start()
    {
        // 처음에는 첫 번째 발판만 활성화
        InitializePlatforms();
        StartCoroutine(PlatformCycle());
    }

    private void InitializePlatforms()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            if (i == 0) // 첫 번째 발판만 활성화
            {
                ActivatePlatform(i);
            }
            else // 나머지는 비활성화
            {
                DeactivatePlatform(i);
            }
        }
    }

    private IEnumerator PlatformCycle()
    {
        while (true)
        {
            // 홀수 인덱스(1, 3, 5...) 활성화하고 짝수 인덱스(0, 2, 4...) 비활성화
            for (int i = 0; i < platforms.Count; i++)
            {
                if (i % 2 == 1) // 홀수 인덱스일 때
                {
                    ActivatePlatform(i);
                }
                else // 짝수 인덱스일 때
                {
                    DeactivatePlatform(i);
                }
            }

            yield return new WaitForSeconds(platformCycleTime / currentSpeed); // 홀수 인덱스 발판 활성화 유지 시간

            // 짝수 인덱스(0, 2, 4...) 활성화하고 홀수 인덱스(1, 3, 5...) 비활성화
            for (int i = 0; i < platforms.Count; i++)
            {
                if (i % 2 == 0) // 짝수 인덱스일 때
                {
                    ActivatePlatform(i);
                }
                else // 홀수 인덱스일 때
                {
                    DeactivatePlatform(i);
                }
            }

            yield return new WaitForSeconds(platformCycleTime / currentSpeed); // 짝수 인덱스 발판 활성화 유지 시간
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
        currentSpeed = Mathf.Clamp(speed, 0.1f, 5f); // 속도 범위 제한
        Debug.Log("Current Platform Speed: " + currentSpeed);
    }
}

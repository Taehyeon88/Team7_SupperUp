using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPuzzle : MonoBehaviour
{
    public Transform hourHand;
    public Transform minuteHand;
    public GameObject bridge;
    public float rotationSpeed = 10f;

    private bool isPuzzleSolved = false;
    private float hourAngle = 0f;
    private float minuteAngle = 0f;

    void Update()
    {
        // 플레이어 입력 처리
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 시침 회전
        if (Input.GetKey(KeyCode.H))
        {
            hourAngle += horizontalInput * rotationSpeed * Time.deltaTime;
            hourHand.rotation = Quaternion.Euler(0, 0, hourAngle);
        }

        // 분침 회전
        if (Input.GetKey(KeyCode.M))
        {
            minuteAngle += verticalInput * rotationSpeed * Time.deltaTime;
            minuteHand.rotation = Quaternion.Euler(0, 0, minuteAngle);
        }

        // 퍼즐 확인
        CheckPuzzleSolution();
    }

    void CheckPuzzleSolution()
    {
        // 8시를 나타내는 각도 (시침: 240도, 분침: 0도)
        bool isHourCorrect = Mathf.Abs(hourAngle % 360 - 240) < 5f;
        bool isMinuteCorrect = Mathf.Abs(minuteAngle % 360) < 5f;

        if (isHourCorrect && isMinuteCorrect && !isPuzzleSolved)
        {
            SolvePuzzle();
        }
        else if ((!isHourCorrect || !isMinuteCorrect) && isPuzzleSolved)
        {
            UnsolvePuzzle();
        }
    }

    void SolvePuzzle()
    {
        isPuzzleSolved = true;
        bridge.SetActive(true);
        Debug.Log("퍼즐이 해결되었습니다! 다리가 나타났습니다.");
    }

    void UnsolvePuzzle()
    {
        isPuzzleSolved = false;
        bridge.SetActive(false);
        Debug.Log("퍼즐이 해제되었습니다. 다리가 사라졌습니다.");
    }
}
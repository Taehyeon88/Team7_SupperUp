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
        // �÷��̾� �Է� ó��
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ��ħ ȸ��
        if (Input.GetKey(KeyCode.H))
        {
            hourAngle += horizontalInput * rotationSpeed * Time.deltaTime;
            hourHand.rotation = Quaternion.Euler(0, 0, hourAngle);
        }

        // ��ħ ȸ��
        if (Input.GetKey(KeyCode.M))
        {
            minuteAngle += verticalInput * rotationSpeed * Time.deltaTime;
            minuteHand.rotation = Quaternion.Euler(0, 0, minuteAngle);
        }

        // ���� Ȯ��
        CheckPuzzleSolution();
    }

    void CheckPuzzleSolution()
    {
        // 8�ø� ��Ÿ���� ���� (��ħ: 240��, ��ħ: 0��)
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
        Debug.Log("������ �ذ�Ǿ����ϴ�! �ٸ��� ��Ÿ�����ϴ�.");
    }

    void UnsolvePuzzle()
    {
        isPuzzleSolved = false;
        bridge.SetActive(false);
        Debug.Log("������ �����Ǿ����ϴ�. �ٸ��� ��������ϴ�.");
    }
}
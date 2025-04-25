using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRotatingCube : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    public float rotationSpeed = 50f; // 회전 속도
    public float moveDistance = 3f; // 이동 거리

    private Vector3 initialPosition;
    private float currentMovement;
    private bool movingRight = true;
    private int currentRotationAxis = 0; // 0: 위, 1: 오른쪽, 2: 아래, 3: 왼쪽
    private bool rotating = false; // 현재 회전 중인지 여부

    void Start()
    {
        initialPosition = transform.position; // 초기 위치 저장
        currentMovement = moveDistance; // 초기 이동 거리 설정
        ChooseRandomRotationAxis(); // 최초 회전 방향 선택
    }

    void Update()
    {
        MoveCube();
        if (!rotating)
        {
            StartCoroutine(RotateCube());
        }
    }

    void MoveCube()
    {
        currentMovement = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        if (movingRight)
        {
            transform.position = initialPosition + new Vector3(currentMovement, 0, 0);
        }
        else
        {
            transform.position = initialPosition + new Vector3(moveDistance - currentMovement, 0, 0);
        }

        // 이동 방향 전환
        if (currentMovement >= moveDistance)
        {
            movingRight = false;
        }
        else if (currentMovement <= 0)
        {
            movingRight = true;
        }
    }

    System.Collections.IEnumerator RotateCube()
    {
        rotating = true; // 회전 중 표시
        float rotationProgress = 0f;
        Vector3 rotationAxis = Vector3.zero;

        // 현재 방향에 대한 회전 설정
        switch (currentRotationAxis)
        {
            case 0: // 위
                rotationAxis = new Vector3(1, 0, 0); // X축 회전
                break;
            case 1: // 오른쪽
                rotationAxis = new Vector3(0, 1, 0); // Y축 회전
                break;
            case 2: // 아래
                rotationAxis = new Vector3(-1, 0, 0); // 반대 방향으로 X축 회전
                break;
            case 3: // 왼쪽
                rotationAxis = new Vector3(0, -1, 0); // 반대 방향으로 Y축 회전
                break;
        }

        // 90도(π/2) 회전하는 동안 루프
        while (rotationProgress < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis, step);
            rotationProgress += step;
            yield return null; // 한 프레임 대기
        }

        // 새로운 회전 방향 선택
        ChooseRandomRotationAxis(); // 무작위 회전 방향 재선정
        rotating = false; // 회전 완료 표시
    }

    void ChooseRandomRotationAxis()
    {
        currentRotationAxis = Random.Range(0, 4); // 0에서 3 범위의 랜덤 값을 선택 (위, 오른쪽, 아래, 왼쪽)
    }
}

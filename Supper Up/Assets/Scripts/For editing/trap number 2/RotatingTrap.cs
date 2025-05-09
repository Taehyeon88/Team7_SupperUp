using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도
    public float wallLength = 1f;      // 벽의 길이 조절 변수
    public float wallHeight = 1f;      // 벽의 높이 조절 변수
    public float wallThickness = 0.1f;  // 벽의 두께 조절 변수

    private GameObject wall;            // 벽을 나타내는 GameObject

    void Start()
    {
        // 벽 생성
        wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.parent = this.transform;    // RotatePlatform의 자식으로 설정
        UpdateWallSize();                          // 초기 벽 크기 설정

        // 오른쪽 끝으로 위치 이동
        wall.transform.localPosition = new Vector3(wallLength / 2, wallHeight / 2, 0);
    }

    void Update()
    {
        // 벽의 크기를 조정하기 위해 매 프레임마다 업데이트
        UpdateWallSize();

        // 벽의 오른쪽 끝을 기준으로 회전
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void UpdateWallSize()
    {
        // 벽의 크기 조정
        wall.transform.localScale = new Vector3(wallLength, wallHeight, wallThickness); // 두께 조정
        // 위치를 오른쪽 끝으로 이동
        wall.transform.localPosition = new Vector3(wallLength / 2, wallHeight / 2, 0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTrap : MonoBehaviour
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float wallLength = 1f;      // ���� ���� ���� ����
    public float wallHeight = 1f;      // ���� ���� ���� ����
    public float wallThickness = 0.1f;  // ���� �β� ���� ����

    private GameObject wall;            // ���� ��Ÿ���� GameObject

    void Start()
    {
        // �� ����
        wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.parent = this.transform;    // RotatePlatform�� �ڽ����� ����
        UpdateWallSize();                          // �ʱ� �� ũ�� ����

        // ������ ������ ��ġ �̵�
        wall.transform.localPosition = new Vector3(wallLength / 2, wallHeight / 2, 0);
    }

    void Update()
    {
        // ���� ũ�⸦ �����ϱ� ���� �� �����Ӹ��� ������Ʈ
        UpdateWallSize();

        // ���� ������ ���� �������� ȸ��
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void UpdateWallSize()
    {
        // ���� ũ�� ����
        wall.transform.localScale = new Vector3(wallLength, wallHeight, wallThickness); // �β� ����
        // ��ġ�� ������ ������ �̵�
        wall.transform.localPosition = new Vector3(wallLength / 2, wallHeight / 2, 0);
    }
}
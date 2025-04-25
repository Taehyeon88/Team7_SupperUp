using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingRotatingCube : MonoBehaviour
{
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    public float moveDistance = 3f; // �̵� �Ÿ�

    private Vector3 initialPosition;
    private float currentMovement;
    private bool movingRight = true;
    private int currentRotationAxis = 0; // 0: ��, 1: ������, 2: �Ʒ�, 3: ����
    private bool rotating = false; // ���� ȸ�� ������ ����

    void Start()
    {
        initialPosition = transform.position; // �ʱ� ��ġ ����
        currentMovement = moveDistance; // �ʱ� �̵� �Ÿ� ����
        ChooseRandomRotationAxis(); // ���� ȸ�� ���� ����
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

        // �̵� ���� ��ȯ
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
        rotating = true; // ȸ�� �� ǥ��
        float rotationProgress = 0f;
        Vector3 rotationAxis = Vector3.zero;

        // ���� ���⿡ ���� ȸ�� ����
        switch (currentRotationAxis)
        {
            case 0: // ��
                rotationAxis = new Vector3(1, 0, 0); // X�� ȸ��
                break;
            case 1: // ������
                rotationAxis = new Vector3(0, 1, 0); // Y�� ȸ��
                break;
            case 2: // �Ʒ�
                rotationAxis = new Vector3(-1, 0, 0); // �ݴ� �������� X�� ȸ��
                break;
            case 3: // ����
                rotationAxis = new Vector3(0, -1, 0); // �ݴ� �������� Y�� ȸ��
                break;
        }

        // 90��(��/2) ȸ���ϴ� ���� ����
        while (rotationProgress < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            transform.Rotate(rotationAxis, step);
            rotationProgress += step;
            yield return null; // �� ������ ���
        }

        // ���ο� ȸ�� ���� ����
        ChooseRandomRotationAxis(); // ������ ȸ�� ���� �缱��
        rotating = false; // ȸ�� �Ϸ� ǥ��
    }

    void ChooseRandomRotationAxis()
    {
        currentRotationAxis = Random.Range(0, 4); // 0���� 3 ������ ���� ���� ���� (��, ������, �Ʒ�, ����)
    }
}

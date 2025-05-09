using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float rotateDegree = 50f; // ȸ�� �ӵ�
    private Quaternion deltaRotation;
    private Vector3 rotateVelocity;

    public enum RotateDirection
    {
        Clockwise,
        Counter_Clockwise
    }

    [SerializeField] private RotateDirection myDir;

    // �÷��̾� ȸ���� ����
    private Quaternion rotationDir;
    private Vector3 dirVec;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (rigid == null)
        {
            Debug.LogError("There is no Rigidbody attached to the RotatingPlatform.");
        }
        else
        {
            InitializeRotation();
        }
    }

    void InitializeRotation()
    {
        // ���� ���� �ʱ�ȭ
        switch (myDir)
        {
            case RotateDirection.Clockwise:
                rotateVelocity = new Vector3(0, rotateDegree, 0);
                rotationDir = Quaternion.Euler(0, -90, 0);
                break;
            case RotateDirection.Counter_Clockwise:
                rotateVelocity = new Vector3(0, -rotateDegree, 0);
                rotationDir = Quaternion.Euler(0, 90, 0);
                break;
        }
    }

    void FixedUpdate()
    {
        if (rigid != null) // Rigidbody�� ���� ��쿡�� ȸ��
        {
            deltaRotation = Quaternion.Euler(rotateVelocity * Time.fixedDeltaTime);
            rigid.MoveRotation(rigid.rotation * deltaRotation);
        }
    }

    // �÷��̾� ȸ���� �Լ�
    public Vector3 GetRotateVec(Vector3 playerPos)
    {
        dirVec = rotationDir * (transform.position - playerPos);
        return dirVec * Time.fixedDeltaTime;
    }
}

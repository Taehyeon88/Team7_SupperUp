using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] private float rotateDegree = 50f; // 회전 속도
    private Quaternion deltaRotation;
    private Vector3 rotateVelocity;

    public enum RotateDirection
    {
        Clockwise,
        Counter_Clockwise
    }

    [SerializeField] private RotateDirection myDir;

    // 플레이어 회전용 변수
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
        // 방향 벡터 초기화
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
        if (rigid != null) // Rigidbody가 있을 경우에만 회전
        {
            deltaRotation = Quaternion.Euler(rotateVelocity * Time.fixedDeltaTime);
            rigid.MoveRotation(rigid.rotation * deltaRotation);
        }
    }

    // 플레이어 회전용 함수
    public Vector3 GetRotateVec(Vector3 playerPos)
    {
        dirVec = rotationDir * (transform.position - playerPos);
        return dirVec * Time.fixedDeltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //카메라 설정 변수
    [Header("Camera Settings")]
    public GameObject player;
    public float cameraDistance = 5f;
    public LayerMask collisionLayer;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    public float mouseSenesitivity = 100.0f;

    private const float Y_ANGLE_MIN = -80.0f;
    private const float Y_ANGLE_MAX = 85.0f;

    public float radius = 5.0f;          //3인칭 카메라와 플레이어 간의 거리

    private bool isShaking = false;
    private float shakingPower = 0;

    void Update()
    {
        CameraRotation();
    }

    //카메라 및 캐릭터 회전처리하는 함수
    public void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSenesitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSenesitivity * Time.deltaTime;

        CurrentX += mouseX;
        CurrentY -= mouseY;

        CurrentY = Mathf.Clamp(CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        //카메라 위치 및 회전 계산
        Vector3 dir = new Vector3(0, 0, -cameraDistance);
        Quaternion rotation = Quaternion.Euler(CurrentY, CurrentX, 0.0f);

        Vector3 origin = player.transform.position + Vector3.up * 1.5f - player.transform.forward * 0.1f;
        Debug.DrawRay(origin, rotation * dir, Color.red);

        if (Physics.SphereCast(origin, 0.2f, rotation * dir, out RaycastHit hit, cameraDistance, collisionLayer))
        {
            float dis = Vector3.Distance(origin, hit.point);

            if (dis < 0.25f) dis = 0.25f;
            dir = new Vector3(0, 0, -(dis - 0.25f));
        }
        else
        {
            if (Physics.CheckSphere(origin, 0.2f, collisionLayer))
            {
                dir = new Vector3(0, 0, -0.1f);
            }
        }

        transform.position = origin + rotation * dir;

        if (isShaking)
        {
            float a = Random.Range(-shakingPower, shakingPower);
            float b = Random.Range(-shakingPower, shakingPower);
            transform.localPosition += new Vector3(a, b, 0);
        }

        transform.LookAt(origin);
    }

    public void StartCameraShake(float power)
    {
        shakingPower = power;
        isShaking = true;
    }

    public void StopCameraShake()
    {
        isShaking = false;
        shakingPower = 0;
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = player.transform.position + Vector3.up * 1.5f - player.transform.forward * 0.1f;
        Gizmos.DrawWireSphere(origin, 0.2f);
    }
}

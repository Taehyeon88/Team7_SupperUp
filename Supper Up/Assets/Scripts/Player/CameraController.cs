using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //카메라 설정 변수
    [Header("Camera Settings")]
    public PlayerController player;
    public float cameraDistance = 5f;
    public LayerMask collisionLayer;

    private float CurrentX = 0.0f;
    private float CurrentY = 45.0f;
    public float mouseSenesitivity { private get; set; }

    private const float Y_ANGLE_MIN = -75.0f;
    private const float Y_ANGLE_MAX = 85.0f;
    private const float ShakeTime = 0.03f;
    private const float MaxCameraDis = 10f;

    public float radius = 5.0f;          //3인칭 카메라와 플레이어 간의 거리

    private bool isShaking = false;
    private bool readyStopShake = false;
    private float shakingPower = 0;
    private float timer = 0;
    private float stateTimer = 0;
    private float defaultDistance = 0f;
    private float currentDis = 0f;

    private void Start()
    {
        defaultDistance = cameraDistance;;

        if (mouseSenesitivity == 0)
            mouseSenesitivity = 100f;
    }
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
        //Debug.DrawRay(origin, rotation * dir, Color.red);

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
            if (!readyStopShake)
            {
                //Debug.Log("떨린다.");

                stateTimer += Time.deltaTime;

                if (stateTimer < 1f) Shake(0.3f, 100f);
                else if (stateTimer < 3f) Shake(0.7f, 1.5f);
                else if (stateTimer < 6f) Shake(1f, 1f);
                else if (stateTimer < 8f) Shake(1.3f, 0.7f);
                else if (stateTimer < 13f) Shake(1.5f, 0.5f);

                cameraDistance = Mathf.Lerp(defaultDistance, MaxCameraDis, player.GetVelocityMegitude() / 20);
                Mathf.Clamp(cameraDistance, defaultDistance, MaxCameraDis);
            }
            else
            {
                stateTimer -= Time.deltaTime;

                Shake(0.3f, 100f);
                cameraDistance = Mathf.Lerp(defaultDistance, currentDis, stateTimer / 2f);
                Mathf.Clamp(cameraDistance, defaultDistance, MaxCameraDis);
                //Debug.Log(cameraDistance);
            }
        }
        transform.LookAt(origin);
    }

    private void Shake(float powerValue, float value)
    {
        timer += Time.deltaTime;

        if (timer > ShakeTime * value)
        {
            float a = Random.Range(-shakingPower * powerValue, shakingPower * powerValue);
            float b = Random.Range(-shakingPower * powerValue, shakingPower * powerValue);
            transform.localPosition += new Vector3(a, b, 0);

            float c = Random.Range(-shakingPower * powerValue, shakingPower * powerValue);
            float d = Random.Range(-shakingPower * powerValue, shakingPower * powerValue);
            player.transform.localPosition += new Vector3(c, 0, d);

            timer = 0;
        }
    }

    public void StartCameraShake(float power)
    {
        shakingPower = power;
        isShaking = true;

        //Debug.Log("떨린다1");
    }

    public void ReadyToStopCameraShake()
    {
        currentDis = defaultDistance;
        readyStopShake = true;
        stateTimer = 2f;
    }

    public void StopCameraShake()
    {
        cameraDistance = defaultDistance;
        isShaking = false;
        readyStopShake = false;
        shakingPower = 0;
        stateTimer = 0;
        timer = 0;
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 origin = player.transform.position + Vector3.up * 1.5f - player.transform.forward * 0.1f;
    //    Gizmos.DrawWireSphere(origin, 0.2f);
    //}
}

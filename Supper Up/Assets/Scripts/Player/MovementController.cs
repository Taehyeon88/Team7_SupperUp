using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Wall Climbing Setting")]
    public float heightValue = 1f;
    public float frontValue = 0.3f;
    public Vector3 boxHalfExtents = Vector3.zero;
    public LayerMask wallLayer;
    private Vector3 targetHandPos;
    [HideInInspector] public bool isClimbing = false;
    [HideInInspector] public float height;
    [HideInInspector] public Vector3 climbDirection;
    public float rayHeight;
    public float rayFront;


    //내부 변수들
    private Rigidbody rb;
    private Animator playerAnimator;
    private PlayerController playerController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    public bool CheckClimbing()
    {
        Vector3 origin = transform.position + transform.up * heightValue + transform.forward * frontValue;
        Collider[] target = Physics.OverlapBox(origin, boxHalfExtents, transform.rotation, wallLayer);

        float climbHeight = origin.y + boxHalfExtents.y;
        if (target.Length >= 1)
        {
            Vector3 rayOrigin = transform.position + transform.up * rayHeight + transform.forward * rayFront;
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 2f))
            {
                height = hit.point.y;
                climbDirection = target[0].transform.position - transform.position;
                climbDirection.y = 0;
                if (height <= climbHeight) return true;
            }
        }
        return false;
    }

    public void StartClimbing()
    {
        playerAnimator.applyRootMotion = true;
        rb.useGravity = false;
        GetComponent<Collider>().isTrigger = true;

        Vector3 rayPos = new Vector3(transform.position.x, height - 0.05f, transform.position.z);

        if (Physics.Raycast(rayPos, climbDirection, out RaycastHit hit2, 2))
        {
            Quaternion targetRot = Quaternion.LookRotation(-hit2.normal, Vector3.up);
            transform.DOLocalRotateQuaternion(targetRot, 0.5f);
        }

        Vector3 Handforward = transform.position + transform.forward * 0.4f;       //캐릭터의 손이 위치할 곳
        targetHandPos = new Vector3(Handforward.x, height + 0.05f, Handforward.z);

        Vector3 temp = new Vector3(0, 1.43f, 0.31f);                               //손의 위치와 offset만큼의 거리가 되게 이동
        Vector3 desiredPos = targetHandPos - transform.rotation * temp;

        transform.DOMove(desiredPos, 0.3f);

        playerController.isOneTime = true;
    }

    public void EndClimbing()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Climbing") && stateInfo.normalizedTime >= 0.7f)
        {
            playerAnimator.applyRootMotion = false;
            rb.velocity = Vector3.zero;
            isClimbing = false;
            rb.useGravity = true;
            GetComponent<Collider>().isTrigger = false;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Climbing") && stateInfo.normalizedTime <= 0.25f)
        {
            SetAnimationWeight(1, 1);
        }
        else if (stateInfo.IsName("Climbing") && stateInfo.normalizedTime > 0.25f)
        {
            float temp = 0.8f - stateInfo.normalizedTime;
            temp = Mathf.Max(temp, 0);
            SetAnimationWeight(temp, temp);
        }
        if (stateInfo.IsName("Climbing"))
        {
            Vector3 leftHandPos = targetHandPos - transform.right * 0.4f;
            Vector3 rightHandPos = targetHandPos + transform.right * 0.4f;
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
        }
    }

    private void SetAnimationWeight(float value1, float value2)
    {
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, value1);
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, value1);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, value2);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, value2);
    }

    private void OnDrawGizmos()
    {
        //플레이어 벽체크용
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * heightValue + transform.forward * frontValue, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

        //플레이어 벽타기 높이 받는용
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * rayHeight + transform.forward * rayFront, transform.rotation, Vector3.one);
        Gizmos.DrawRay(Vector3.zero, Vector3.down);
    }
}

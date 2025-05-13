using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
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
    [HideInInspector] public bool isOneTime = false;
    public float rayHeight;
    public float rayFront;


    //���� ������
    private Rigidbody rb;
    private Animator playerAnimator;
    private PlayerController playerController;

    private bool endClimbing = false;

    private Vector3 test;
    private Vector3 test2;

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
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 2f))                    //����ĳ��Ʈ�� ��Ÿ������ ������ �޴´�.
            {
                test = origin + Vector3.up * boxHalfExtents.y;
                test2 = hit.point;
                height = hit.point.y;
                Debug.Log($"�繰 ���� : {hit.point.y}, ��Ÿ�Ⱑ�� ���� : {climbHeight}");

                if (Vector3.Angle(Vector3.up, hit.normal) <= playerController.maxSlopeAngle + 2f)
                {
                    climbDirection = target[0].transform.position - transform.position;
                    climbDirection.y = 0;
                    if (height <= climbHeight) return true;
                }
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

        Vector3 Handforward = transform.position + transform.forward * 0.4f;       //ĳ������ ���� ��ġ�� ��
        targetHandPos = new Vector3(Handforward.x, height + 0.05f, Handforward.z);

        Vector3 temp = new Vector3(0, 1.43f, 0.31f);                               //���� ��ġ�� offset��ŭ�� �Ÿ��� �ǰ� �̵�
        Vector3 desiredPos = targetHandPos - transform.rotation * temp;

        transform.DOMove(desiredPos, 0.3f);

        isOneTime = true;
    }

    public void EndClimbing()
    {
        AnimatorTransitionInfo transInfo = playerAnimator.GetAnimatorTransitionInfo(0);
        if (transInfo.IsName("Climbing -> Movement"))
        {
            endClimbing = true;
        }
        else if (endClimbing)
        {
            playerAnimator.applyRootMotion = false;
            rb.velocity = Vector3.zero;
            isClimbing = false;
            rb.useGravity = true;
            GetComponent<Collider>().isTrigger = false;
            endClimbing = false;
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

    private void OnAnimatorMove()
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Climbing"))
        {
            Vector3 delta = playerAnimator.deltaPosition;

            if (isOneTime)
            {
                delta -= Vector3.up * 0.1f;
                isOneTime = false;
            }

            rb.MovePosition(rb.position + delta);
            rb.velocity = Vector3.zero;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(test, 0.1f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(test2, 0.1f);

        //�÷��̾� ��üũ��
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * heightValue + transform.forward * frontValue, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);

        //�÷��̾� ��Ÿ�� ���� �޴¿�
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * rayHeight + transform.forward * rayFront, transform.rotation, Vector3.one);
        Gizmos.DrawRay(Vector3.zero, Vector3.down);
    }
}

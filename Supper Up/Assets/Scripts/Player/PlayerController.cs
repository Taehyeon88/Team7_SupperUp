using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;
    //���� ������
    private Vector3 movement = Vector3.zero;

    //�÷��̾��� ������ �ӵ��� �����ϴ� ����
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float jumpfrontForce = 2.5f;
    public float rotationSpeed = 10f;     //ȸ���ӵ�
    private float currentSpeed = 0;
    private float currentRotateSpeed = 0;

    [Header("Player Rotation")]
    public float velocity = 1;
    public float max_velocity = 3f;
    private float min_velocity = 1f;
    private float speedTimer = 0;
    //���ڸ� ȸ������
    private float rotateTimer = 0;
    public float rotateTime = 3f;
    public float rotateDegree = 30;
    Quaternion toRoation = Quaternion.identity;
    private float moveDegree = 0;
    public bool onRotate = false;

    [Header("Veriable for Ground Check")]
    private bool wasGrounded = false;
    public float maxSlopeAngle = 30f;
    public Vector3 groundHalfExtents;
    public LayerMask groundLayer;

    [HideInInspector] public bool isFalling = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isLanding = false;
    [HideInInspector] public bool isHightLanding = false;

    //���� ������
    private Rigidbody rb;
    private Animator playerAnimator;
    private RaycastHit slopeHit;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;          //���콺 Ŀ���� ��װ� �����

        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        currentRotateSpeed = rotationSpeed;
    }

    //�÷��̾� �ൿó�� �Լ�
    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //�¿� �Է�(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //�յ� �Է�(1, -1)

        float temp = Mathf.Max(Mathf.Abs(moveVertical), Mathf.Abs(moveHorizontal));

        ConstraintsMove();

        //�̵� ���� ���
        if (!onRotate) movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized * temp;  //�밢�̵���, ����(����ó��)
        else movement = transform.forward * moveVertical;
        moveDegree = movement.magnitude;

        if (CheckHitWall(movement.normalized))                                                         //���� �����Ÿ� �̻��� ���, �̵����߱�(��������������)
        {
            //Debug.Log("������ ������!");
            AdjustSpeed(moveVertical, true);
            movement = Vector3.zero;
        }
        else
        {
            AdjustSpeed(moveVertical, false);
            RotateDiagonal(moveHorizontal, moveVertical, movement);
        }
        
        playerAnimator.SetFloat("FMove", moveVertical * velocity);                          //�ִϸ��̼�
        playerAnimator.SetFloat("RMove", moveHorizontal);

        bool isOnSlope = IsOnSlope();                                                       //����̵���, �ڵ�
        movement = isOnSlope ? AdjustDirectionToSlope(movement.normalized) : movement;

        rb.MovePosition(rb.position + movement * (moveSpeed + velocity) * Time.deltaTime);

    }

    private bool IsOnSlope()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        
        if (Physics.Raycast(origin, Vector3.down, out slopeHit, 0.5f, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0 && angle < maxSlopeAngle; 
        }
        return false;
    }

    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void ConstraintsMove() //���߿� ���� ��, �̼Ӱ���.
    {
        if (CheckDistance() > 1.1f && !IsGrounded())
        {
            moveSpeed = 0.1f;
            velocity = min_velocity;
            speedTimer = 0;
        }
        else moveSpeed = currentSpeed;
    }
    //����, ����, �������� ���� �̵��� �ʱ�ȭ
    public void ResetVelocity()
    {
        velocity = min_velocity;
        speedTimer = 0;
    }

    //�ð��� �帧�� ���� �ӵ��� �÷��ִ� �ڵ�
    private void AdjustSpeed(float moveVertical, bool isDownVelocity)   //1�� �Ű����� : ��ǲ�� ���� ��, �ӵ����� | 2�� �Ű����� 
    {
        if (isDownVelocity) speedTimer = Mathf.Max(speedTimer -= Time.deltaTime * 2f, 0);
        else
        {
            if (Mathf.Abs(moveVertical) > 0.95f)
            {
                speedTimer = Mathf.Min(speedTimer += Time.deltaTime * 1.2f, max_velocity);
            }
            else speedTimer = Mathf.Max(speedTimer -= Time.deltaTime * 2f, 0);
        }
        velocity = Mathf.Clamp(speedTimer, min_velocity, max_velocity);
    }

    public void Rotate(bool OnRotateAni)
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (moveDegree > 0.1 && !onRotate)                                      //ī�޶� ���� ĳ����ȸ��
        {
            toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
            rotationSpeed = currentRotateSpeed;
        }

        float dot = Vector3.Dot(transform.forward, cameraForward);               //ȸ��ó���� �� �����ϱ�
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle >= rotateDegree && moveDegree < 0.1)                           //n���Ŀ� ī�޶�������� ȸ��
        {
            rotateTimer += Time.deltaTime;
            if (rotateTimer > rotateTime)
            {
                toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
                rotationSpeed = currentRotateSpeed / (angle / 90);
                rotateTimer = 0;
                Vector3 left = -transform.right;
                float temp = Vector3.Angle(cameraForward, left);

                if (OnRotateAni)
                {
                    if (temp <= 90) playerAnimator.SetTrigger("Lturn");
                    else if (temp <= 180) playerAnimator.SetTrigger("Rturn");
                }
            }
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, toRoation, rotationSpeed * Time.deltaTime);
    }

    private void RotateDiagonal(float moveHorizontal, float moveVertical, Vector3 movement)
    {
        if (!onRotate)
        {
            if (velocity < 1.5 || Mathf.Abs(moveHorizontal) < 0.3f || Mathf.Abs(moveVertical) < 0.9f) return;
            Vector3 temp = movement;
            if (moveVertical < -0.9f) temp = -temp;
            toRoation = Quaternion.LookRotation(temp, Vector2.up);
            rotationSpeed = currentRotateSpeed;
            onRotate = true;
        }
        else
        {
            if (Mathf.Abs(moveHorizontal) > 0.9f && Mathf.Abs(moveVertical) > 0.3f) return;
            onRotate = false;
        }
    }

    public void Jumping()
    {
        rb.velocity = Vector3.up * jumpForce + movement * velocity * jumpfrontForce;
    }

    public void Landing(bool isSupper)
    {
        if (isSupper)
        {
            float fallingSpeed = Mathf.Abs(rb.velocity.y);
            playerAnimator.SetFloat("LandSpeed", Mathf.Clamp(fallingSpeed / 10, 0.8f, 2));
            isFalling = false;
        }
        else
        {
            float fallingSpeed = Mathf.Abs(rb.velocity.y);
            playerAnimator.SetFloat("LandSpeed", Mathf.Clamp(fallingSpeed / 15, 1f, 2));
        }
    }
   public void CheckLanding()
   {
        Debug.Log("������� : " + CheckDistance());
        if (isJumping)
        {
            if (IsGrounded() && !wasGrounded)  //������, �������
            {
                isLanding = true;
            }
        }
        else if (CheckDistance() < 2.2f && !IsGrounded() && CheckFalling(1) && !isHightLanding && CheckDistance() > 1.3f)  //�̵���, ���� ������ �������
        {
            isHightLanding = true;
            //Debug.Log("�������� : " + CheckDistance());
            //Debug.Log("�Ϲ����� Ȱ��ȭ");
        }

        if (isLanding || isHightLanding)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jumping Down") && stateInfo.normalizedTime > 0.3)
            {
                isLanding = false;
                isHightLanding = false;
            }
        }

        wasGrounded = IsGrounded();
   }

    public float CheckDistance()
    {
        RaycastHit hit;
        Vector3 temp = transform.position + Vector3.up * 1;
        if (Physics.Raycast(temp, Vector3.down, out hit)) return hit.distance;
        return 10f;
    }

    public bool IsGrounded()
    {
        //Debug.Log("�ٴڿ���üũ: " + Physics.CheckBox(transform.position, groundHalfExtents, Quaternion.identity, groundLayer));
        return Physics.CheckBox(transform.position, groundHalfExtents, Quaternion.identity, groundLayer);
    }
    public bool CheckFalling(float value)
    {
        if (rb.velocity.y < - value) return true;
        return false;
    }

    private bool CheckHitWall(Vector3 movement)
    {
        float distance = 0.32f;

        List<Vector3> rayPos = new List<Vector3>();
        rayPos.Add(transform.position + transform.up * 0.3f);
        rayPos.Add(transform.position + transform.up * 0.8f);
        rayPos.Add(transform.position + transform.up * 1.3f);
        rayPos.Add(transform.position + transform.up * 1.8f);

        foreach (var origin in rayPos)
        {
            Debug.DrawRay(origin, movement * distance, Color.red);
            if (Physics.Raycast(origin, movement, distance))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        //�÷��̾� �ٴ�üũ��

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, groundHalfExtents * 2);
        //Gizmos.DrawWireSphere(start, radius);
    }

}

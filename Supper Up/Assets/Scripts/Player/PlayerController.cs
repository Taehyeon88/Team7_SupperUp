using System.Collections;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera thirdPersonCamera;
    //내부 변수들
    private Vector3 movement = Vector3.zero;

    //플레이어의 움직임 속도를 설정하는 변수
    [Header("Player Movement")]
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float rotationSpeed = 10f;     //회전속도
    private float currentSpeed = 0;
    private float currentRotateSpeed = 0;

    [Header("Player Rotation")]
    public float velocity = 1;
    public float max_velocity = 3f;
    private float min_velocity = 1f;
    private float speedTimer = 0;
    //제자리 회전변수
    private float rotateTimer = 0;
    public float rotateTime = 3f;
    public float rotateDegree = 30;
    Quaternion toRoation = Quaternion.identity;
    private float moveDegree = 0;
    public bool onRotate = false;

    [Header("Veriable for Ground Check")]
    public bool isFalling = false;
    public bool isJumping = false;
    private bool wasGrounded = false;
    public bool isLanding = false;
    public bool startLanding = false;
    public LayerMask groundLayer;

    [Header("Wall Climbing Setting")]
    public float heightValue = 1f;
    public float frontValue = 0.3f;
    public Vector3 boxHalfExtents = Vector3.zero;
    public LayerMask wallLayer;
    public bool isClimbing = false;
    public Vector3 climbOffset;
    private Vector3 targetHandPos;
    private Vector3 targetPos;
    private float timer = 0f;


    //내부 변수들
    private Rigidbody rb;
    private Animator playerAnimator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;          //마우스 커서를 잠그고 숨긴다

        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        currentRotateSpeed = rotationSpeed;
    }

    void Update()
    {
        Landing();
        Climbing();
    }

    //플레이어 행동처리 함수
    public void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");         //좌우 입력(1, -1)
        float moveVertical = Input.GetAxis("Vertical");             //앞뒤 입력(1, -1)

        ConstraintsMove();

        AdjustSpeed(moveVertical);
        //애니메이션
        playerAnimator.SetFloat("FMove", moveVertical * velocity);
        playerAnimator.SetFloat("RMove", moveHorizontal);

        //이동 백터 계산
        if (!onRotate) movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        else movement = transform.forward * moveVertical;
        moveDegree = movement.magnitude;

        rb.MovePosition(rb.position + movement * (moveSpeed + velocity) * Time.deltaTime);

        // 추가된 미끄러짐 효과 코드
        // 미끄러짐 효과 추가
        if (IsOnIce()) // 빙판 위에 있을 경우
        {
            Vector3 slipForce = movement * (moveSpeed * 50f) * Time.deltaTime; // moveSpeed를 사용하여 미끄러지는 힘을 추가
            rb.AddForce(slipForce, ForceMode.Acceleration);
        }

        RotateDiagonal(moveHorizontal, moveVertical, movement);
    }

    // 플레이어가 빙판길에 있는지 확인하는 메서드 추가
    private bool IsOnIce()
    {
        // Raycast를 사용하여 아래에 있는 오브젝트의 레이어를 확인
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }


    public void ConstraintsMove() //공중에 있을 시, 이속감소.
    {
        if (CheckDistance() > 1.1f)
        {
            moveSpeed = 0.1f;
            velocity = min_velocity;
            speedTimer = 0;
        }
        else moveSpeed = currentSpeed;
    }
    //파쿠르, 착지, 슈퍼착지 이후 이동값 초기화
    public void ResetVelocity()
    {
        velocity = min_velocity;
        speedTimer = 0;
    }

    //시간이 흐름에 따라 속도를 올려주는 코드
    private void AdjustSpeed(float moveVertical)
    {
        if (Mathf.Abs(moveVertical) > 0.95f)
        {
            speedTimer = Mathf.Min(speedTimer += Time.deltaTime * 1.2f, max_velocity);
        }
        else
        {
            speedTimer = Mathf.Max(speedTimer -= Time.deltaTime * 2f, 0);
        }
        velocity = Mathf.Clamp(speedTimer, min_velocity, max_velocity);
    }

    public void Rotate(bool OnRotateAni)
    {
        Vector3 cameraForward = thirdPersonCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        if (moveDegree > 0.1 && !onRotate)                                      //카메라에 따라서 캐릭터회전
        {
            toRoation = Quaternion.LookRotation(cameraForward, Vector2.up);
            rotationSpeed = currentRotateSpeed;
        }

        float dot = Vector3.Dot(transform.forward, cameraForward);               //회전처리를 할 각구하기
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle >= rotateDegree && moveDegree < 0.1)                           //n초후에 카메라방향으로 회전
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

    public void RotateDiagonal(float moveHorizontal, float moveVertical, Vector3 movement)
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
        rb.AddForce(Vector3.up * jumpForce + movement * velocity * 2.5f, ForceMode.Impulse);
    }

    public void SupperLanding()
    {
        float fallingSpeed = Mathf.Abs(rb.velocity.y);
        playerAnimator.SetFloat("LandSpeed", Mathf.Clamp(fallingSpeed / 10, 0.8f, 2));
        isFalling = false;
    }
   public void Landing()
   {

        if (IsGrounded() && !wasGrounded && isJumping)
        {
            //Debug.Log("된다");
            isLanding = true;
            isJumping = false;
        }
        if (isLanding)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Jumping Down") && stateInfo.normalizedTime > 0.3)
            {
                isLanding = false;
            }
            else if(stateInfo.IsName("Movement")) isLanding = false;
        }

        wasGrounded = IsGrounded();
   }

    public void Climbing()
    {
        Vector3 origin = transform.position + transform.up * heightValue + transform.forward * frontValue;
        Collider[] target = Physics.OverlapBox(origin, boxHalfExtents, transform.rotation, wallLayer);

        float climbHeight = origin.y + boxHalfExtents.y;
        if (target.Length >= 1)
        {
            //Debug.Log($"{target[0].transform.position.y}, {target[0].transform.localScale.y / 2}");
            float height = target[0].transform.position.y + target[0].transform.localScale.y / 2;
            if (height <= climbHeight && !isClimbing)
            {
                //isClimbing = true;
                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;
                //playerAnimator.applyRootMotion = true;
                //GetComponent<Collider>().isTrigger = true;

                Vector3 forward = transform.position + transform.forward * 0.5f;           //캐릭터가 이동할 위치
                targetPos = new Vector3(forward.x, height + 0.05f, forward.z);

                Vector3 Handforward = transform.position + transform.forward * 0.5f;       //캐릭터의 손이 위치할 곳
                targetHandPos = new Vector3(Handforward.x, height + 0.05f, Handforward.z);

                Vector3 temp = new Vector3(0, 1.43f, 0.31f);                               //손의 위치와 offset만큼의 거리가 되게 이동
                Vector3 desiredPos = targetHandPos - transform.rotation * temp;

                //transform.position = Vector3.Lerp(transform.position, desiredPos, timer += Time.deltaTime);

                Vector3 original = new Vector3(transform.position.x, height - 0.05f, transform.position.z);
                /*
                if (Physics.Raycast(original, transform.forward, out RaycastHit hit, 2))
                {
                    toRoation = Quaternion.LookRotation(-hit.normal, Vector2.up);
                    rotationSpeed = currentRotateSpeed;

                    if (Vector3.Angle(transform.forward, -hit.normal) < 0.3f)
                    {
                        transform.position = desiredPos;
                        isClimbing = true;
                        playerAnimator.applyRootMotion = true;
                        GetComponent<Collider>().isTrigger = true;
                    }
                }*/
            }
        }
        if (isClimbing)
        { 
            timer += Time.deltaTime;
            if (timer >= 2.4f)                                   //애니메이션이 끝났을 때, 초기화
            {
                transform.position = targetPos;
                playerAnimator.applyRootMotion = false;
                timer = 0;
                Debug.Log("된다");
                GetComponent<Collider>().isTrigger = false;
                isClimbing = false;
            }
        }
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
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float radius = 0.3f;
        return Physics.CheckSphere(origin, radius, groundLayer);
    }
    
    private void OnAnimatorIK(int layerIndex)
    {
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Climbing"))
        {
            Vector3 leftHandPos = targetHandPos - transform.right * 0.4f;
            Vector3 rightHandPos = targetHandPos + transform.right * 0.4f;

            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);

            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
        }
        else
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0f);
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
        }

    }

    private void OnDrawGizmos()
    {
        //플레이어 바닥체크용
        Vector3 start = transform.position + Vector3.up * 0.1f;
        float radius = 0.3f;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(start, radius);

        //플레이어 벽체크용

        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * heightValue + transform.forward * frontValue, transform.rotation, Vector3.one);

        Vector3 origin = transform.position + transform.up * heightValue + transform.forward * frontValue;
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }

}

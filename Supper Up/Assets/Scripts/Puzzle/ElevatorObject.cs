using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ElevatorObject : MonoBehaviour
{
    [Header("Move Info")]
    [SerializeField] private Transform endPos;
    [SerializeField] private Ease moveEase;
    [SerializeField] private float duration;

    //플레이어를 감지할 박스의 위치와 크기변수
    [Header("Checking box Info")]
    [SerializeField] private float heightValue = 1f;
    [SerializeField] private Vector3 boxHalfExtents;
    [SerializeField] private LayerMask playerLayer;

    [Header("door Info")]
    [SerializeField] private GameObject door;
    [SerializeField] private Ease openEase;
    [SerializeField] private float openDuration;

    //내부변수
    private Rigidbody rb;
    private Vector3 startPos;
    private Rigidbody playerRb;
    private Vector3 currenPos;
    private Vector3 prevPos;
    private float timer;

    private bool isMoving = false;
    private bool isOneTime = false;
    private bool isPlayerLeave = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerRb = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update()
    {
        if (CheckPlayer() && !isOneTime)
        {
            isOneTime = true;
            StartCoroutine(OperateSeauence());
        }

        if (!isOneTime) return;

        if (isPlayerLeave)
        {
            CheckPlayerLeave();
        }
        else
        {
            timer = 0;
        }
    }

    private IEnumerator OperateSeauence()
    {
        CloseDoor();
        yield return new WaitUntil(() => isMoving);
        yield return new WaitForSeconds(1f);
        prevPos = transform.position;
        OnElevator();
        yield return new WaitUntil(() => !isMoving);
        OpenDoor();
        yield return new WaitUntil(() => isMoving);
        CloseDoor();
        ResetObject();
        yield return new WaitUntil(() => !isMoving);
        OpenDoor();
        isOneTime = false;
    }

    private bool CheckPlayer()  //플레어감지(탑승확인)
    {
        Vector3 origin = transform.position + Vector3.up * heightValue;
        return Physics.CheckBox(origin, boxHalfExtents, Quaternion.identity, playerLayer);
    }

    private void OnElevator()
    {
        transform.DOMove(endPos.position, duration)
            .SetEase(moveEase)
            .SetUpdate(UpdateType.Fixed)
            .OnUpdate(() => {

                currenPos = transform.position;
                Vector3 dir = currenPos - prevPos;
                playerRb.MovePosition(playerRb.position +  dir);

                prevPos = currenPos;
            
            })
            .OnComplete(() => isMoving = false);
    }

    private void CloseDoor()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(-door.transform.right, Vector2.up);
        door.transform.DORotateQuaternion(rotateQuaternion, openDuration)
            .SetEase(openEase)
            .OnComplete(() => isMoving = true);
    }

    private void OpenDoor()
    {
        Quaternion rotateQuaternion = Quaternion.LookRotation(door.transform.right, Vector2.up);
        door.transform.DORotateQuaternion(rotateQuaternion, openDuration)
            .SetEase(openEase);
    }

    private void ResetObject()
    {
        transform.DOMove(startPos, duration)
            .SetEase(moveEase)
            .OnComplete(() => isMoving = false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerLeave = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerLeave = false;
        }
    }

    private void CheckPlayerLeave()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            isMoving = true;
            timer = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //플레이어 벽체크용
        Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * heightValue, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}

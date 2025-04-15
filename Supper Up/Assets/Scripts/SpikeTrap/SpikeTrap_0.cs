using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap_0 : MonoBehaviour
{
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float startMoveDistance;
    [SerializeField] private float pushForce;
    [SerializeField] private float frontValue;
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] private Ease PushSpickEase;
    [SerializeField] private Ease PullSpickEase;
    [SerializeField] private LayerMask playerMask;
    
    //내부변수
    private Vector3 originalPos;
    private Vector3 targetPos;
    private PlayerController player;
    private bool startMove = false;
    private bool isPushing = true;
    private Coroutine currentCoroutine;

    void Start()
    {
        originalPos = transform.position;
        targetPos = transform.position + transform.forward * moveDistance;
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();                 //플레이어와의 거리계산 함수
        PushPlayer();                    //플레이어가 충돌시 밀어내는 함수
    }

    private void CheckDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(originalPos, player.transform.position);
            if (distance < startMoveDistance - 1 && !startMove)
            {
                currentCoroutine = StartCoroutine(StartThrust());
                startMove = true;
            }
            else if (distance > startMoveDistance + 1 && startMove)
            {
                Debug.Log("된다");
                EndThrust();
                startMove = false;
            }
        }
    }

    private IEnumerator StartThrust()           //장애물 실행함수
    {
        while (true)
        {
            switch(isPushing)
            {
                case true:
                    transform.DOMove(targetPos, moveSpeed).SetAutoKill(false).SetEase(PushSpickEase).OnComplete(()=> isPushing = false);
                break;
                case false:
                    transform.DOMove(originalPos, moveSpeed).SetAutoKill(false).SetEase(PullSpickEase).OnComplete(() => isPushing = true);
                break;
            }
            yield return null;
        }
    }
    private void EndThrust()           //장애물 종료함수
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    private void PushPlayer()
    {
        Vector3 origin = transform.position + transform.forward * frontValue;
        Collider[] target = Physics.OverlapBox(origin, halfExtents, Quaternion.identity, playerMask);
        if (target.Length > 0)
        {
            Rigidbody rb = target[0].gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);

        }

    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + transform.forward * frontValue;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * moveDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startMoveDistance - 1);
        Gizmos.DrawWireCube(origin, halfExtents);
    }
}

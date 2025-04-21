using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //작동시작거리
    [SerializeField] private float pushForce;          //밀쳐내는 힘
    [SerializeField] private float frontValue;         //플레이어감지 범위설정
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] protected LayerMask playerMask;     //플레이어감지

    //내부변수
    protected Vector3 originalPos;
    private PlayerController player;
    private bool startMove = false;
    protected virtual void Start()
    {
        originalPos = transform.position;
        player = FindObjectOfType<PlayerController>();
    }

    protected virtual void Update()
    {
        CheckDistance();
        PushPlayer();                    //플레이어가 충돌시 밀어내는 함수
    }

    private void CheckDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(originalPos, player.transform.position);
            if (distance < startMoveDistance - 1 && !startMove)
            {
                StartThrust();
                startMove = true;
            }
            else if (distance > startMoveDistance + 1 && startMove)
            {
                //Debug.Log("된다");
                EndThrust();
                startMove = false;
            }
        }
    }
    private void PushPlayer()
    {
        Vector3 origin = transform.position + transform.forward * frontValue;
        Collider[] target = Physics.OverlapBox(origin, halfExtents, transform.rotation, playerMask);
        if (target.Length > 0)
        {
            Rigidbody rb = target[0].gameObject.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
    }

    protected virtual void StartThrust() { }          //장애물 실행함수
    protected virtual void EndThrust() { }            //장애물 종료함수


    protected virtual void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + transform.forward * frontValue;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startMoveDistance - 1);

        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
    }
}

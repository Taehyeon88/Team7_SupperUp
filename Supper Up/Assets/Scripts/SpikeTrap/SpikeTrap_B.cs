using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //작동시작거리
    [SerializeField] private float pushForce;          //밀쳐내는 힘
    [SerializeField] protected LayerMask playerMask;     //플레이어감지

    //내부변수
    protected Vector3 originalPos;
    private PlayerController player;
    private bool startMove = false;
    protected Rigidbody rb;
    private Rigidbody playerRb;
    protected virtual void Start()
    {
        originalPos = transform.position;
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        playerRb = player.GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        CheckDistance();
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerRb.velocity = transform.forward * pushForce;
            player.isTrusted = true;
        }
    }

    protected virtual void StartThrust() { }          //장애물 실행함수
    protected virtual void EndThrust() { }            //장애물 종료함수


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startMoveDistance - 1);
    }
}

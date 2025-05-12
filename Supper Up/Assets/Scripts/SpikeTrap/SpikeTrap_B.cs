using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //�۵����۰Ÿ�
    [SerializeField] private float pushForce;          //���ĳ��� ��
    [SerializeField] protected LayerMask playerMask;     //�÷��̾��

    //���κ���
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
                //Debug.Log("�ȴ�");
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

    protected virtual void StartThrust() { }          //��ֹ� �����Լ�
    protected virtual void EndThrust() { }            //��ֹ� �����Լ�


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startMoveDistance - 1);
    }
}

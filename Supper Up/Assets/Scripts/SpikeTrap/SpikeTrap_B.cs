using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //�۵����۰Ÿ�
    [SerializeField] private float pushForce;          //���ĳ��� ��
    [SerializeField] private float frontValue;         //�÷��̾�� ��������
    [SerializeField] private Vector3 halfExtents;
    [SerializeField] protected LayerMask playerMask;     //�÷��̾��

    //���κ���
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
        PushPlayer();                    //�÷��̾ �浹�� �о�� �Լ�
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
                Debug.Log("�ȴ�");
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

    protected virtual void StartThrust() { }          //��ֹ� �����Լ�
    protected virtual void EndThrust() { }            //��ֹ� �����Լ�


    protected virtual void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + transform.forward * frontValue;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startMoveDistance - 1);

        Gizmos.matrix = Matrix4x4.TRS(origin, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);
    }
}

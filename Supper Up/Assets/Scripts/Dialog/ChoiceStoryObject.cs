using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ChoiceStoryObject : MonoBehaviour
{
    [Header("Defualt Info")]
    public int choiceObjectId;
    public TextMeshProUGUI choiceText;
    public bool isOneTime = false;
    public bool useElevator = false;


    [Header("Move Info")]
    public float endHight = 5f;
    public Ease moveEase;
    public float duration;

    //�÷��̾ ������ �ڽ��� ��ġ�� ũ�⺯��
    [Header("Checking box Info")]
    [SerializeField] private float heightValue = 1f;
    [SerializeField] private Vector3 boxHalfExtents;
    [SerializeField] private LayerMask playerLayer;

    //���κ���
    private Vector3 startPos;
    private Vector3 endPos;
    private Rigidbody playerRb;
    private Vector3 currenPos;
    private Vector3 prevPos;
    private float timer;

    private bool isMoving = false;
    private bool isOneTime2 = false;
    private bool isPlayerLeave = false;

    void Start()
    {
        playerRb = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>();
        startPos = transform.position;
        endPos = transform.position + Vector3.up * endHight;
    }

    void Update()
    {
        if (!useElevator) return;

        if (CheckPlayer() && !isOneTime2)
        {
            isOneTime2 = true;
            StartCoroutine(OperateSeauence());
        }

        if (!isOneTime2) return;

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
        isMoving = true;
        yield return new WaitForSeconds(1f);
        prevPos = transform.position;
        OnElevator();
        isMoving = false;
        yield return new WaitUntil(() => isMoving);
        ResetObject();
        yield return new WaitUntil(() => !isMoving);
        isOneTime2 = false;
    }

    private bool CheckPlayer()  //�÷����(ž��Ȯ��)
    {
        Vector3 origin = transform.position + Vector3.up * heightValue;
        return Physics.CheckBox(origin, boxHalfExtents, Quaternion.identity, playerLayer);
    }

    private void OnElevator()
    {
        transform.DOMove(endPos, duration)
            .SetEase(moveEase)
            .SetUpdate(UpdateType.Fixed)
            .OnUpdate(() => {

                currenPos = transform.position;
                Vector3 dir = currenPos - prevPos;
                playerRb.MovePosition(playerRb.position + dir);

                prevPos = currenPos;

            })
            .OnComplete(() => isMoving = false);
    }

    private void ResetObject()
    {
        transform.DOMove(startPos, duration)
            .SetEase(moveEase)
            .OnComplete(() => isMoving = false);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerLeave = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isOneTime)
        {
            if (StoryManager.Instance != null)
            {
                StoryManager.Instance.SelectChoice(choiceObjectId);
            }
            isOneTime = true;
        }

        if (collision.collider.CompareTag("Player"))
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
        Gizmos.DrawSphere(transform.position + Vector3.up * endHight, 0.5f);

        //�÷��̾� ��üũ��
        Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * heightValue, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}

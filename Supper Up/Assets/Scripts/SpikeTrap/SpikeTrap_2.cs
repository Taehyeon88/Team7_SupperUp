using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_2 : SpikeTrap_B
{
    [Header("SpikeTrap_1 Variable")]
    [SerializeField] private float pushTime;      //�տ��� �̵��ð�
    [SerializeField] private float pullTime;      //�ڿ��� �̵��ð�
    [SerializeField] private float rayDistance;    //���̰����Ÿ�
    [SerializeField] private float heightValue;     //���� ���̳���
    [SerializeField] private Ease PushSpickEase;   //��ġ�� �ִϸ��̼�
    [SerializeField] private Ease PullSpickEase;   //�ǵ����� �ִϸ��̼�

    [Header("SpikeTrap_2 Variable")]
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveTime;
    [SerializeField] private Ease moveEase;

    //���κ���
    private Vector3 CheckPoint_1;
    private Vector3 CheckPoint_2;
    private bool isMoving = true;
    private Coroutine currentCoroutine;
    private Tween MoveTween_1;
    private Tween MoveTween_2;
    //������_1
    private Sequence sequence;
    private bool isChecking = false;
    private bool isPushing = false;
    private bool isOneTime = false;

    Vector3 targetPos;
    Vector3 currentPos;

    protected override void Start()
    {
        base.Start();
        CheckPoint_1 = transform.position + transform.right * moveDistance;
        CheckPoint_2 = transform.position;
        SetTweens();
    }


    protected override void Update()
    {
        base.Update();
        CheckPlayer();
    }
    protected override void StartThrust()
    {
        isChecking = true;
        currentCoroutine = StartCoroutine(C_StartThrust());
    }

    private IEnumerator C_StartThrust()                           //��ֹ� �����Լ�
    {
        while (true)
        {
            Debug.Log(isPushing);
            if (isPushing)
            {
                isMoving = true;
                isOneTime = false;
                yield return null;
                continue;
            }
            switch (isMoving)
            {
                case true:
                    if (MoveTween_1 != null && !isOneTime)
                    {
                        MoveTween_1.Restart();
                        isOneTime = true;
                    }
                    break;
                case false:
                    if (MoveTween_2 != null && isOneTime)
                    {
                        MoveTween_2.Restart();
                        isOneTime = false;
                    }
                    break;
            }
            yield return null;
        }
    }

    protected override void EndThrust()
    {
        Debug.Log("����ȴ�");
        isChecking = false;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
    private void CheckPlayer()
    {
        if (!isChecking || isPushing) return;

        Vector3 origin = transform.position + transform.up * heightValue;

        if (Physics.Raycast(origin, transform.forward, rayDistance, playerMask))
        {
            isPushing = true;
            StopTween();
            PlaySequence();
        }
    }
    private void SetTweens()
    {
        transform.DOMove(CheckPoint_1, 1);

        MoveTween_1 = transform.DOMove(CheckPoint_1, moveTime)
                        .SetAutoKill(false)
                        .SetEase(moveEase)
                        .OnComplete(() => isMoving = false);
        MoveTween_2 = transform.DOMove(CheckPoint_2, moveTime)
                        .SetAutoKill(false)
                        .SetEase(moveEase)
                        .OnComplete(() => isMoving = true);
        MoveTween_1.Pause();
        MoveTween_2.Pause();
    }

    private void StopTween()
    {
        MoveTween_1.Pause();
        MoveTween_2.Pause();
    }

    private void PlaySequence()
    {
        sequence = DOTween.Sequence();

        targetPos = transform.position + transform.forward * rayDistance;
        currentPos = transform.position;

        sequence.Append(transform.DOMove(targetPos, pushTime).SetEase(PushSpickEase))
                .Append(transform.DOMove(currentPos, pullTime).SetEase(PullSpickEase))
                .OnComplete(() => isPushing = false);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        Vector3 origin = transform.position + transform.up * heightValue;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right * moveDistance);
    }
}

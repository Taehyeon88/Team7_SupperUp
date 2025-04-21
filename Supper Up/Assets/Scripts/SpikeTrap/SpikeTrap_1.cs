using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap_1 : SpikeTrap_B
{
    [Header("SpikeTrap_1 Variable")]
    [SerializeField] private float pushTime;      //�տ��� �̵��ð�
    [SerializeField] private float pullTime;      //�ڿ��� �̵��ð�
    [SerializeField] private float rayDistance;    //���̰����Ÿ�
    [SerializeField] private float heightValue;     //���� ���̳���
    [SerializeField] private Ease PushSpickEase;   //��ġ�� �ִϸ��̼�
    [SerializeField] private Ease PullSpickEase;   //�ǵ����� �ִϸ��̼�

    //���κ���
    private Vector3 targetPos;
    private Sequence sequence;
    private bool isChecking = false;
    private bool isPushing = false;

    private void Awake()
    {
        sequence = DOTween.Sequence();
    }
    protected override void Start()
    {
        base.Start();
        targetPos = transform.position + transform.forward * rayDistance;
        SetSequence();
    }
    protected override void Update()
    {
        base.Update();
        CheckPlayer();
    }
    protected override void StartThrust()  //���̷� �÷��̾��
    {
        isChecking = true;
    }
    protected override void EndThrust()
    {
        isChecking = false;
    }

    private void CheckPlayer()
    {
        if (!isChecking || isPushing) return;

        Vector3 origin = transform.position + transform.up * heightValue;

        if (Physics.Raycast(origin, transform.forward, rayDistance, playerMask))
        {
            sequence.Restart();
            isPushing = true;
        }
    }

    private void SetSequence()
    {

        Tween pushTween = transform.DOMove(targetPos, pushTime)
                        .SetAutoKill(false)
                        .SetEase(PushSpickEase);
        Tween pullTween = transform.DOMove(originalPos, pullTime)
                        .SetAutoKill(false)
                        .SetEase(PullSpickEase);

        sequence.Append(pushTween).Append(pullTween).SetAutoKill(false).Pause().OnComplete(()=> isPushing = false);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        Vector3 origin = transform.position + transform.up * heightValue;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(origin, transform.forward * rayDistance);
    }
}

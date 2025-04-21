using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap_0 : SpikeTrap_B
{
    [Header("SpikeTrap_0 Variable")]
    [SerializeField] private float moveDistance;   //�յ��̵��Ÿ�
    [SerializeField] private float pushTime;      //�տ��� �̵��ð�
    [SerializeField] private float pullTime;      //�ڿ��� �̵��ð�
    [SerializeField] private Ease PushSpickEase;   //��ġ�� �ִϸ��̼�
    [SerializeField] private Ease PullSpickEase;   //�ǵ����� �ִϸ��̼�
    
    //���κ���
    private Vector3 targetPos;
    private bool isPushing = true;
    private bool isOneTime = false;
    private Coroutine currentCoroutine;
    private Tween pushTween;
    private Tween pullTween;

    protected override void Start()
    {
        base.Start();
        targetPos = transform.position + transform.forward * moveDistance;
        SetTweens();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void StartThrust()
    {
        currentCoroutine = StartCoroutine(C_StartThrust());
    }

    private IEnumerator C_StartThrust()                     //��ֹ� �����Լ�
    {
        Debug.Log("����ȴ�");
        while (true)
        {
            switch(isPushing)
            {
                case true:
                    if (pushTween != null && !isOneTime)
                    {
                        pushTween.Restart();
                        isOneTime = true;
                    }
                break;
                case false:
                    if (pushTween != null && isOneTime)
                    {
                        pullTween.Restart();
                        isOneTime = false;
                    }
                    break;
            }
            yield return null;
        }
    }

    protected override void EndThrust()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    private void SetTweens()
    {
        pushTween = transform.DOMove(targetPos, pushTime)
                        .SetAutoKill(false)
                        .SetEase(PushSpickEase)
                        .OnComplete(() => isPushing = false);
        pullTween = transform.DOMove(originalPos, pullTime)
                        .SetAutoKill(false)
                        .SetEase(PullSpickEase)
                        .OnComplete(() => isPushing = true);
        pushTween.Pause();
        pullTween.Pause();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(originalPos, transform.forward * moveDistance);
    }
}

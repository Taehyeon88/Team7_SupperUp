using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap_0 : SpikeTrap_B
{
    [Header("SpikeTrap_0 Variable")]
    [SerializeField] private float moveDistance;   //앞뒤이동거리
    [SerializeField] private float pushTime;      //앞오로 이동시간
    [SerializeField] private float pullTime;      //뒤오로 이동시간
    [SerializeField] private Ease PushSpickEase;   //밀치기 애니메이션
    [SerializeField] private Ease PullSpickEase;   //되돌오기 애니메이션
    
    //내부변수
    private Vector3 targetPos;
    private bool isPushing = true;
    private bool isOneTime = false;
    private bool isThrusting = false;
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

    private void FixedUpdate()
    {
        Thrust();
    }
    protected override void StartThrust()
    {
        isThrusting = true;
    }

    protected override void EndThrust()
    {
        isThrusting = false;
    }

    private void Thrust()
    {
        if (isThrusting)
        {
            switch (isPushing)  //장애물의 밀치기실행
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
        }
    }

    private void SetTweens()
    {
        //pushTween = DOTween.To(
        //    () => rb.position,
        //    x => rb.MovePosition(x),
        //    targetPos,
        //    pushTime
        //    ).SetAutoKill(false)
        //     .SetEase(PushSpickEase)
        //     .OnComplete(() => isPushing = false)
        //     .SetUpdate(UpdateType.Fixed);

        //pullTween = DOTween.To(
        //    () => rb.position,
        //    x => rb.MovePosition(x),
        //    originalPos,
        //    pullTime
        //    ).SetAutoKill(false)
        //     .SetEase(PullSpickEase)
        //     .OnComplete(() => isPushing = true)
        //     .SetUpdate(UpdateType.Fixed);

        pushTween = rb.DOMove(targetPos, pushTime)
            .SetAutoKill(false)
            .SetEase(PushSpickEase)
            .OnComplete(() => isPushing = false)
            .SetUpdate(UpdateType.Fixed);

        pullTween = rb.DOMove(originalPos, pullTime)
            .SetAutoKill(false)
            .SetEase(PullSpickEase)
            .OnComplete(() => isPushing = true)
            .SetUpdate(UpdateType.Fixed);

        pushTween.Pause();
        pullTween.Pause();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * moveDistance);
    }
}

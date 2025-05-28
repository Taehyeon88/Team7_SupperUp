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
            switch (isPushing)  //��ֹ��� ��ġ�����
            {
                case true:
                    if (pushTween != null && !isOneTime)
                    {
                        if (SoundManager.instance != null)
                        {
                            SoundManager.instance.FadeSound_S(audioSources[pushId], 1f);
                            SoundManager.instance.FadeSound_S(audioSources[pullId], 0f, true);
                        }
                        pushTween.Restart();
                        isOneTime = true;
                    }
                    break;
                case false:
                    if (pullTween != null && isOneTime)
                    {
                        if (SoundManager.instance != null)
                        {
                            SoundManager.instance.FadeSound_S(audioSources[pullId], 1f);
                            SoundManager.instance.FadeSound_S(audioSources[pushId], 0f, true);
                        }
                        pullTween.Restart();
                        isOneTime = false;
                    }
                    break;
            }
        }
    }

    private void SetTweens()
    {
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

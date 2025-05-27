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

    private Vector3 startPos;

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
        SoundManager.instance.FadeSound_S(audioSources[rayId], 1f);
        isChecking = true;
    }
    protected override void EndThrust()
    {
        SoundManager.instance.FadeSound_S(audioSources[rayId], 0f);
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

            SoundManager.instance.FadeSound_S(audioSources[pushId], 1f);
            SoundManager.instance.FadeSound_S(audioSources[pullId], 0f, true);
        }
    }

    private void SetSequence()
    {

        Tween pushTween = rb.DOMove(targetPos, pushTime)
            .SetAutoKill(false)
            .SetEase(PushSpickEase)
            .OnComplete(() => { 
                isPushing = false;
                SoundManager.instance.FadeSound_S(audioSources[pullId], 1f);
                SoundManager.instance.FadeSound_S(audioSources[pushId], 0f, true);}
            )
            .SetUpdate(UpdateType.Fixed);

        Tween pullTween = rb.DOMove(originalPos, pullTime)
            .SetAutoKill(false)
            .SetEase(PullSpickEase)
            .OnComplete(() => { 
                isPushing = true;
                SoundManager.instance.FadeSound_S(audioSources[pullId], 0f, true);}
            )
            .SetUpdate(UpdateType.Fixed);

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

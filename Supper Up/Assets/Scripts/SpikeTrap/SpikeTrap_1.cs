using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap_1 : SpikeTrap_B
{
    [Header("SpikeTrap_1 Variable")]
    [SerializeField] private float pushTime;      //앞오로 이동시간
    [SerializeField] private float pullTime;      //뒤오로 이동시간
    [SerializeField] private float rayDistance;    //레이감지거리
    [SerializeField] private float heightValue;     //레이 높이낮이
    [SerializeField] private Ease PushSpickEase;   //밀치기 애니메이션
    [SerializeField] private Ease PullSpickEase;   //되돌오기 애니메이션

    //내부변수
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
    protected override void StartThrust()  //레이로 플레이어감지
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

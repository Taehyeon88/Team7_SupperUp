using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_2 : SpikeTrap_1
{
    [Header("SpikeTrap_2 Variable")]
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveTime;
    [SerializeField] private Ease moveEase;

    //내부변수
    private Vector3 CheckPoint_1;
    private Vector3 CheckPoint_2;
    private bool isPushing = true;
    private Coroutine currentCoroutine;
    private Tween pushTween;
    private Tween pullTween;

    protected override void Start()
    {
        base.Start();
        CheckPoint_1 = transform.position + transform.right * moveDistance;
        CheckPoint_2 = transform.position - transform.right * moveDistance;

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

    private IEnumerator C_StartThrust()                     //장애물 실행함수
    {
        base.StartThrust();
        Debug.Log("실행된다");
        while (true)
        {
            if (isPushing)
            {
                if (pushTween != null) pushTween.Restart();
                yield return new WaitUntil(() => !isPushing);
            }
            else
            {
                if (pushTween != null) pullTween.Restart();
                yield return new WaitUntil(() => isPushing);
            }
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
        transform.DOMove(CheckPoint_1, 1);

        pushTween = transform.DOMove(CheckPoint_1, moveTime)
                        .SetAutoKill(false)
                        .SetEase(moveEase)
                        .OnComplete(() => isPushing = false);
        pullTween = transform.DOMove(originalPos, moveTime)
                        .SetAutoKill(false)
                        .SetEase(moveEase)
                        .OnComplete(() => isPushing = true);
        pushTween.Pause();
        pullTween.Pause();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right * moveDistance);
        Gizmos.DrawRay(transform.position, - transform.right * moveDistance);
    }
}

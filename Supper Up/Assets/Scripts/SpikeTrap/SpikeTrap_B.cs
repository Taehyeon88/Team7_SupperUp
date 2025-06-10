using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //작동시작거리
    [SerializeField] private float pushForce;          //밀쳐내는 힘
    [SerializeField] protected LayerMask playerMask;     //플레이어감지
    [SerializeField] private GameObject detectPrefab;    //플레이어 감지 블럭

    //내부변수
    protected Vector3 originalPos;
    private PlayerController player;
    private bool startMove = false;
    protected Rigidbody rb;
    private Rigidbody playerRb;

    protected List<AudioSource> audioSources = new List<AudioSource>();
    protected int rayId;
    protected int pushId;
    protected int pullId;

    protected virtual void Start()
    {
        originalPos = transform.position;
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        playerRb = player.GetComponent<Rigidbody>();

        AudioSource[] t_audioSources = GetComponents<AudioSource>();

        if (t_audioSources.Length <= 0) return;

        for (int i = 0; i < t_audioSources.Length; i++)
        {
            audioSources.Add(t_audioSources[i]);
            string name = SoundManager.instance.FindAudioWithClip(t_audioSources[i].clip);

            switch (name)
            {
                case "Ray": rayId = i; break;
                case "Push": pushId = i; break;
                case "Pull": pullId = i; break;
            }
        }
    }

    protected virtual void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(detectPrefab.transform.position, player.transform.position);
            if (distance < startMoveDistance - 1 && !startMove)
            {
                StartThrust();
                startMove = true;
            }
            else if (distance > startMoveDistance + 1 && startMove)
            {
                EndThrust();
                startMove = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerRb.velocity = transform.forward * pushForce;
            player.isTrusted = true;
        }
    }

    protected virtual void StartThrust() { }          //장애물 실행함수
    protected virtual void EndThrust() { }            //장애물 종료함수


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectPrefab.transform.position, startMoveDistance - 1);
    }
}

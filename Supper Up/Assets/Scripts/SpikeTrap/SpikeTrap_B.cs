using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class SpikeTrap_B : MonoBehaviour
{
    [Header("Base SpikeTrap Variable")]
    [SerializeField] private float startMoveDistance;  //�۵����۰Ÿ�
    [SerializeField] private float pushForce;          //���ĳ��� ��
    [SerializeField] protected LayerMask playerMask;     //�÷��̾��
    [SerializeField] private GameObject detectPrefab;    //�÷��̾� ���� ��

    //���κ���
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

    protected virtual void StartThrust() { }          //��ֹ� �����Լ�
    protected virtual void EndThrust() { }            //��ֹ� �����Լ�


    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectPrefab.transform.position, startMoveDistance - 1);
    }
}

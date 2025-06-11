using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Transparentlegs : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField]
    private float fallDelay = 2f;        // �÷��̾� ���� �� ������� �ð�
    [SerializeField]
    private float respawnDelay = 5f;     // ������� �ٽ� ��Ÿ���� �ð�

    private bool isTriggered = false;    // �ߺ� ������

    // ���� ��ġ ���� (�ʿ��)
    private Vector3 originalPosition;
    public Collider[] colliders;
    public MeshRenderer[] renderers;
    public AudioClip playClip;           //�μ����� ���� Ŭ��

    void Start()
    {
        originalPosition = transform.position;
    }

    // �浹 ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(DestroyAndRespawn());
        }
    }

    private IEnumerator DestroyAndRespawn()
    {
        isTriggered = true;

        // ���� �ð� �Ŀ� �ٸ� �����
        yield return new WaitForSeconds(fallDelay);

        //Debug.Log(respawnDelay);
        if (SoundManager.instance != null) SoundManager.instance.PlayAllSoundWithClip(playClip);

        for (int i = 0; i < renderers.Length; i++)
        {
            colliders[i].enabled = false;
            renderers[i].enabled = false;
        }

        // ���� �ð� �� �ٽ� Ȱ��ȭ (�ٽ� ����)
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log(respawnDelay);

        isTriggered = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            colliders[i].enabled = true;
            renderers[i].enabled = true;
        }
    }

    private void OnEnable()
    {
        // ������Ʈ ��Ȱ��ȭ �� �� �ʱ�ȭ
        isTriggered = false;
        // �ʿ�� ��ġ, ���� �ʱ�ȭ �� �߰� ����
        // transform.position = originalPosition;  // ��ġ �ʱ�ȭ
    }
}

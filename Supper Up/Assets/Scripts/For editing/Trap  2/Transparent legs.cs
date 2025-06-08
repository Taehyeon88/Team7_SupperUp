using System.Collections;
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
    private Collider collider;
    private MeshRenderer renderer;

    void Start()
    {
        originalPosition = transform.position;
        collider = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    // �浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
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

        collider.enabled = false;
        renderer.enabled = false;

        // ���� �ð� �� �ٽ� Ȱ��ȭ (�ٽ� ����)
        yield return new WaitForSeconds(respawnDelay);

        Debug.Log(respawnDelay);

        isTriggered = false;

        collider.enabled = true;
        renderer.enabled = true;
    }

    private void OnEnable()
    {
        // ������Ʈ ��Ȱ��ȭ �� �� �ʱ�ȭ
        isTriggered = false;
        // �ʿ�� ��ġ, ���� �ʱ�ȭ �� �߰� ����
        // transform.position = originalPosition;  // ��ġ �ʱ�ȭ
    }
}

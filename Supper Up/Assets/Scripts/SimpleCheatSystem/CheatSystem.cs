using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatSystem : MonoBehaviour
{
    public static CheatSystem instance { get; private set; }

    [Header("UI ���۷���")]
    public GameObject cheatPanel; // ġƮ UI �г�
    public TMP_InputField commandInput; // ��ɾ� �Է�â
    public TextMeshProUGUI outputText; // ��� �ؽ�Ʈ

    [Header("�÷��̾� ����")]
    public Transform playerTransform; // �÷��̾� Transform
    public Rigidbody playerRigidbody; // �÷��̾� Rigidbody

    private Vector3 startPosition; // ���� ���� �� ��ġ ����
    private bool isPanelActive = false; // ġƮâ ���� ����
    private bool isSpeedBoosted = false; // �ӵ� ���� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (playerTransform != null)
                startPosition = playerTransform.position; // ���� ��ġ ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ���� �� ġƮâ �� ���̰�
        if (cheatPanel != null)
            cheatPanel.SetActive(false);
        Log("ġƮ �ý��� �غ� �Ϸ�. F1 Ű�� ����");
    }

    private void Update()
    {
        // F1 Ű�� ������ �г� ���
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isPanelActive = !isPanelActive; // ���

            if (cheatPanel != null)
                cheatPanel.SetActive(isPanelActive); // ǥ��/�����

            if (isPanelActive && commandInput != null)
                commandInput.ActivateInputField();
        }

        // ġƮâ �����ְ� Enter ������ ��ɾ� ó��
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand();
        }
    }

    public void ExecuteCommand()
    {
        string cmd = commandInput.text.Trim().ToLower();
        if (string.IsNullOrEmpty(cmd))
            return;

        Log("> " + cmd);

        if (cmd.StartsWith("fly"))
        {
            ToggleFly();
        }
        else if (cmd.StartsWith("home"))
        {
            ReturnHome();
        }
        else if (cmd.StartsWith("reset"))
        {
            ResetPlayerPosition();
        }
        else
        {
            Log("�� �� ���� ��ɾ�");
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    void ToggleFly()
    {
        if (playerRigidbody != null)
        {
            if (playerRigidbody.useGravity)
            {
                playerRigidbody.useGravity = false;
                Debug.Log("�÷��̾ ���� �����մϴ�");
            }
            else
            {
                playerRigidbody.useGravity = true;
                Debug.Log("�÷��̾� ���� ����");
            }
        }
        else
        {
            Debug.Log("�÷��̾� Rigidbody�� ã�� �� ����");
        }
    }

    void ReturnHome()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Debug.Log("ó�� ��ġ�� ���ư�");
        }
        else
        {
            Debug.Log("�÷��̾� ���� ����");
        }
    }

    void ResetPlayerPosition()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Debug.Log("�÷��̾� ��ġ�� ���� ��ġ�� ����");
        }
        else
        {
            Debug.Log("�÷��̾� ���� ����");
        }
    }

    private void Log(string message, bool isError = false)
    {
        string msg = isError ? $"<color=red>{message}</color>" : message;
        if (outputText != null)
        {
            outputText.text += msg + "\n";
        }
        else
        {
            Debug.Log(message);
        }
    }
}

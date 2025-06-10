using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// �ڷ���Ʈ ��ġ�� �����ϴ� Ŭ����
[System.Serializable]
public class TeleportPoint
{
    public string name;     // "1telp", "2telp" ��
    public Vector3 position; // ��ġ ��ǥ
}

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

    // �ν����Ϳ��� ���� ������ �ڷ���Ʈ ��ġ ����Ʈ
    public List<TeleportPoint> teleportPoints;

    // ���� ��ųʸ� ��ȯ��
    private Dictionary<string, Vector3> teleportPositionDict = new Dictionary<string, Vector3>();

    private Vector3 startPosition; // ���� ��ġ ����
    private bool isPanelActive = false; // ġƮâ Ȱ��ȭ ����
    private bool isFlying = false; // ���� ��� ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // ���� ��ġ ����
            if (playerTransform != null)
                startPosition = playerTransform.position;

            // ����Ʈ�� ��ųʸ��� ��ȯ
            foreach (var tp in teleportPoints)
            {
                teleportPositionDict[tp.name.ToLower()] = tp.position;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (cheatPanel != null)
            cheatPanel.SetActive(false);
        Log("ġƮ �ý��� �غ� �Ϸ�. F1 Ű�� ����");
    }

    private void Update()
    {
        // F1 Ű�� ġƮ �г� ���
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isPanelActive = !isPanelActive;
            if (cheatPanel != null)
            {
                cheatPanel.SetActive(isPanelActive);
                if (isPanelActive && commandInput != null)
                {
                    commandInput.ActivateInputField();
                    ClearOutputText();
                }
            }
        }

        // ġƮâ ���� �ְ� ���� ������ ��� ����
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand();
        }

        // ���� ����� ��
        if (isFlying)
        {
            FlyMode();
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
        else if (teleportPoints.Exists(tp => tp.name.ToLower() == cmd))
        {
            TeleportToPosition(cmd);
        }
        else if (cmd.StartsWith("help"))
        {
            ShowHelp();
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
            isFlying = !isFlying;
            playerRigidbody.useGravity = !isFlying;
            string flyStatus = isFlying ? "����" : "����";
            Log($"�÷��̾� ���� ��� {flyStatus}");
        }
        else
        {
            Log("�÷��̾� Rigidbody�� ã�� �� ����", true);
        }
    }

    void FlyMode()
    {
        // ī�޶� �Ǵ� �÷��̾��� �� ���� ����
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // ���� ������θ� ��� (Y ���� ����)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // ������ ���� �ʱ�ȭ
        Vector3 moveDirection = Vector3.zero;

        // ���� ���
        if (Input.GetKey(KeyCode.Space))
        {
            playerTransform.position += Vector3.up * Time.deltaTime * 10f; // ��� �ӵ�
        }
        // �Ʒ��� ��������
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerTransform.position += Vector3.down * Time.deltaTime * 10f; // �ϰ� �ӵ�
        }

        // WASD ���� �̵�
        float moveSpeed = 10f;
        if (Input.GetKey(KeyCode.W))
            moveDirection += forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= forward;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += right;

        // ���� ����ȭ�Ͽ� ������ �ӵ� ����
        if (moveDirection != Vector3.zero)
            moveDirection.Normalize();

        // ���� �̵�
        playerTransform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    void ReturnHome()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Log("ó�� ��ġ�� ���ư�");
        }
        else
        {
            Log("�÷��̾� ���� ����", true);
        }
    }

    void TeleportToPosition(string command)
    {
        string key = command.ToLower();
        if (teleportPositionDict.TryGetValue(key, out Vector3 targetPosition))
        {
            playerTransform.position = targetPosition;
            Log($"�÷��̾ {key} ��ġ�� �̵��߽��ϴ�");
        }
        else
        {
            Log("�̵��� ��ġ�� ã�� �� ����", true);
        }
    }

    void ShowHelp()
    {
        Log("��� ������ ��ɾ�:");
        Log("fly - ���� ��� ���");
        Log("home - ó�� ��ġ�� �̵�");
        foreach (var tp in teleportPoints)
        {
            Log($"{tp.name} - �ش� ��ġ�� �̵�");
        }
        Log("help - ���� ǥ��");
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

    private void ClearOutputText()
    {
        if (outputText != null)
        {
            outputText.text = ""; // �ؽ�Ʈ ����
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 텔레포트 위치를 저장하는 클래스
[System.Serializable]
public class TeleportPoint
{
    public string name;     // "1telp", "2telp" 등
    public Vector3 position; // 위치 좌표
}

public class CheatSystem : MonoBehaviour
{
    public static CheatSystem instance { get; private set; }

    [Header("UI 레퍼런스")]
    public GameObject cheatPanel; // 치트 UI 패널
    public TMP_InputField commandInput; // 명령어 입력창
    public TextMeshProUGUI outputText; // 출력 텍스트

    [Header("플레이어 참조")]
    public Transform playerTransform; // 플레이어 Transform
    public Rigidbody playerRigidbody; // 플레이어 Rigidbody

    // 인스펙터에서 설정 가능한 텔레포트 위치 리스트
    public List<TeleportPoint> teleportPoints;

    // 내부 딕셔너리 변환용
    private Dictionary<string, Vector3> teleportPositionDict = new Dictionary<string, Vector3>();

    private Vector3 startPosition; // 시작 위치 저장
    private bool isPanelActive = false; // 치트창 활성화 여부
    private bool isFlying = false; // 비행 모드 여부

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // 시작 위치 저장
            if (playerTransform != null)
                startPosition = playerTransform.position;

            // 리스트를 딕셔너리로 변환
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
        Log("치트 시스템 준비 완료. F1 키로 열기");
    }

    private void Update()
    {
        // F1 키로 치트 패널 토글
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

        // 치트창 열려 있고 엔터 누르면 명령 실행
        if (isPanelActive && Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteCommand();
        }

        // 비행 모드일 때
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
            Log("알 수 없는 명령어");
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
            string flyStatus = isFlying ? "시작" : "종료";
            Log($"플레이어 비행 모드 {flyStatus}");
        }
        else
        {
            Log("플레이어 Rigidbody를 찾을 수 없음", true);
        }
    }

    void FlyMode()
    {
        // 카메라 또는 플레이어의 앞 방향 벡터
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // 수평 평면으로만 계산 (Y 성분 제거)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // 움직임 벡터 초기화
        Vector3 moveDirection = Vector3.zero;

        // 위로 상승
        if (Input.GetKey(KeyCode.Space))
        {
            playerTransform.position += Vector3.up * Time.deltaTime * 10f; // 상승 속도
        }
        // 아래로 내려가기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerTransform.position += Vector3.down * Time.deltaTime * 10f; // 하강 속도
        }

        // WASD 방향 이동
        float moveSpeed = 10f;
        if (Input.GetKey(KeyCode.W))
            moveDirection += forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= forward;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += right;

        // 방향 정규화하여 일정한 속도 유지
        if (moveDirection != Vector3.zero)
            moveDirection.Normalize();

        // 최종 이동
        playerTransform.position += moveDirection * moveSpeed * Time.deltaTime;
    }


    void ReturnHome()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Log("처음 위치로 돌아감");
        }
        else
        {
            Log("플레이어 참조 없음", true);
        }
    }

    void TeleportToPosition(string command)
    {
        string key = command.ToLower();
        if (teleportPositionDict.TryGetValue(key, out Vector3 targetPosition))
        {
            playerTransform.position = targetPosition;
            Log($"플레이어가 {key} 위치로 이동했습니다");
        }
        else
        {
            Log("이동할 위치를 찾을 수 없음", true);
        }
    }

    void ShowHelp()
    {
        Log("사용 가능한 명령어:");
        Log("fly - 비행 모드 토글");
        Log("home - 처음 위치로 이동");
        foreach (var tp in teleportPoints)
        {
            Log($"{tp.name} - 해당 위치로 이동");
        }
        Log("help - 도움말 표시");
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
            outputText.text = ""; // 텍스트 비우기
        }
    }
}

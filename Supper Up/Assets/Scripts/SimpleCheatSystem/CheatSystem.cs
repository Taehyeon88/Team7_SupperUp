using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private Vector3 startPosition; // 게임 시작 시 위치 저장
    private bool isPanelActive = false; // 치트창 열림 여부
    private bool isSpeedBoosted = false; // 속도 증가 여부

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (playerTransform != null)
                startPosition = playerTransform.position; // 시작 위치 저장
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 시작 시 치트창 안 보이게
        if (cheatPanel != null)
            cheatPanel.SetActive(false);
        Log("치트 시스템 준비 완료. F1 키로 열기");
    }

    private void Update()
    {
        // F1 키를 누르면 패널 토글
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isPanelActive = !isPanelActive; // 토글

            if (cheatPanel != null)
                cheatPanel.SetActive(isPanelActive); // 표시/숨기기

            if (isPanelActive && commandInput != null)
                commandInput.ActivateInputField();
        }

        // 치트창 열려있고 Enter 누르면 명령어 처리
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
            Log("알 수 없는 명령어");
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
                Debug.Log("플레이어가 날기 시작합니다");
            }
            else
            {
                playerRigidbody.useGravity = true;
                Debug.Log("플레이어 날기 종료");
            }
        }
        else
        {
            Debug.Log("플레이어 Rigidbody를 찾을 수 없음");
        }
    }

    void ReturnHome()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Debug.Log("처음 위치로 돌아감");
        }
        else
        {
            Debug.Log("플레이어 참조 없음");
        }
    }

    void ResetPlayerPosition()
    {
        if (playerTransform != null)
        {
            playerTransform.position = startPosition;
            Debug.Log("플레이어 위치를 최초 위치로 복구");
        }
        else
        {
            Debug.Log("플레이어 참조 없음");
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

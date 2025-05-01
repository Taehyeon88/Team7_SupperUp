using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    //Tile�� ����
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button ExitButton;

    //Pause�� ����
    [SerializeField] private Button reGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton2;
    [SerializeField] private Button exitGameButton;

    //���κ�����
    private Scene currentScene;
    private bool onSetting = false;
    private bool endPause = false;
    private GameObject pauseCanvas;

    //�ѹ��� ����
    private bool isOneTime = false;
    private bool isOneTime2 = false;

    //Pause�� Ȱ��ȭ���޺���
    [HideInInspector] public bool onPause = false;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        currentScene = SceneManager.GetActiveScene();
        Initialize_T();
        SetButtonDeligate_T();
    }

    void Update()
    {
        CheckPauseCondition();

        //�����Կ���
        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 0f;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)  //�ʱ�ȭ�Լ�
    {
        currentScene = scene;
        endPause = false;

        Time.timeScale = 1f;  //������ �� ���� �ʱ�ȭ
        onPause = false;

        if (scene.name == "TitleScene") Initialize_T();
        else if (scene.name == "ProtoTypeScene")
        {
            Initialize_P();
        }
    }

    private void Initialize_T()     //Ÿ��Ʋ �� �ʱ�ȭ �Լ�
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        settingButton = GameObject.Find("SettingButton").GetComponent<Button>();
        ExitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        SetButtonDeligate_T();
    }
    private void Initialize_P()   //���� �÷��� �� �ʱ�ȭ �Լ�
    {
        pauseCanvas = GameObject.Find("PauseCanvas");
        reGameButton = GameObject.Find("ReGameButton").GetComponent<Button>();
        continueButton = GameObject.Find("ContinueButton").GetComponent<Button>();
        settingButton2 = GameObject.Find("SettingButton").GetComponent<Button>();
        exitGameButton = GameObject.Find("ExitGameButton").GetComponent<Button>();
        reGameButton.onClick.AddListener(StartPlay);
        continueButton.onClick.AddListener(ContinueGame);
        settingButton2.onClick.AddListener(OnSettingMenu);
        exitGameButton.onClick.AddListener(GoToTitle);
        pauseCanvas.SetActive(false);
    }


    private void SetButtonDeligate_T()  //Ui ��ư Ŭ�� ��������Ʈ ����Լ�
    {
        if (currentScene.name == "TitleScene")
        {
            playButton.onClick.AddListener(StartPlay);
            settingButton.onClick.AddListener(OnSettingMenu);
            ExitButton.onClick.AddListener(Exit);
        }
    }

    private void StartPlay()  //���� �����Լ�
    {
        SceneManager.LoadScene("ProtoTypeScene");
    }

    private void OnSettingMenu()  //���� �����Լ�
    {

    }

    private void Exit()   //���������Լ�
    {
        Application.Quit();
    }

    private void CheckPauseCondition()
    {
        if (currentScene.name == "ProtoTypeScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!onPause) PauseGame();
                else ContinueGame();
            }
        }
    }

    private void PauseGame()   //���� �Ͻ����� �Լ�
    {
        Cursor.lockState = CursorLockMode.None;

        pauseCanvas.SetActive(true);
        onPause = true;
        Time.timeScale = 0f;
    }

    private void ContinueGame()   //���� ����ϱ� �Լ�
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseCanvas.SetActive(false);
        onPause = false;
        Time.timeScale = 1f;
    }

    private void GoToTitle()   //Ÿ��Ʋȭ������ �̵��Լ�
    {
        SceneManager.LoadScene("TitleScene");
    }
}

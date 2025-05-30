using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance {  get; private set; }

    [Header("Dialog References")]
    [SerializeField] private DialogDatebaseSO dialogDatabase;

    [Header("UI References")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private List<TextMeshProUGUI> choiceText = new List<TextMeshProUGUI>();

    //[Header("TriggerObjects References")] //선택 혹은 나레이션 대상 오브젝트 리스트
    //[SerializeField] List<GameObject> dialogTriggerObjects = new List<GameObject>();
    
    [SerializeField] List<GameObject> choiceObjects = new List<GameObject>();

    [Header("Dialog Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypewriterEffect = true;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private DialogSO currentDialog;
    private int currentDialogId = 1;

    private int lastDialogNextId = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        if (dialogDatabase != null)
        {
            dialogDatabase.Initialize();
        }
        else
        {
            Debug.LogError("Dialog Database is not assigned to Dialog Manager");
        }
        
        //따로 나레이션이 나오게 할 트리거 추가

        //나레이션 오브젝트 자동할당 메서드 추가
        //선택 오브젝트, 텍스트 자동할당 메서드 추가

        //해당 오브젝트 --> NextDialog() 호출
    }
    public void StartDialog(int dialogId)
    {
        if (dialogId != currentDialogId) return;  //순서에 따라서 작동

        if (lastDialogNextId < 0)                 //선택지가 나올 시, 선택으로 넘어감
        {
            if (currentDialog.choices != null && currentDialog.choices.Count > 0)
            {
                ShowChoices();
            }
            return;
        }

        currentDialogId++;

        DialogSO dialog = dialogDatabase.GetDialogById(dialogId);
        if (dialog != null)
        {
            StartDialog(dialog);
        }
        else
        {
            Debug.LogError($"Dialog with ID {dialogId} not found!");
        }
    }

    public void StartDialog(DialogSO dialog)
    {
        if (dialog == null) return;

        lastDialogNextId = dialog.nextId;    //마지막 next 아이디로 추가

        currentDialog = dialog;
        ShowDialog();
        dialogPanel.SetActive(true);
    }

    public void ShowDialog()
    {
        if (currentDialog == null) return;

        if (useTypewriterEffect)
        {
            StartTypingEffect(currentDialog.text);
        }
        else
        {
            dialogText.text = currentDialog.text;    //나레이션 텍스트 설정
        }

        //선택지 표시
        ClearChoices();
    }
    private void CloseDialog()
    {
        dialogPanel.SetActive(false);
        currentDialog = null;
        StopTypingEffect();
    }

    //텍스트 타이핑 효과 코루틴
    private IEnumerator TypeText(string text)
    {
        dialogText.text = "";
        foreach (char c in text)
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        yield return new WaitForSeconds(10f);
        CloseDialog();
    }

    //타이핑 효과 중지
    private void StopTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void StartTypingEffect(string text)
    {
        isTyping = true;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    public void ClearChoices()
    {
        //선택글끄기
    }

    public void SelectChoice(DialogChoiceSO choice)
    {
        if (choice != null && choice.nextId > 0)
        {
            DialogSO nextDialog = dialogDatabase.GetDialogById(choice.nextId);
            if (nextDialog != null)
            {
                currentDialog = nextDialog;
                ShowDialog();
            }
            else
            {
                CloseDialog();
            }
        }
        else
        {
            CloseDialog();
        }
    }

    //선택지 표시
    private void ShowChoices()
    {
        foreach (var choice in currentDialog.choices)
        {
            

            //선택지 보여주기
        }
    }
}

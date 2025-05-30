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

    //[Header("TriggerObjects References")] //���� Ȥ�� �����̼� ��� ������Ʈ ����Ʈ
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
        
        //���� �����̼��� ������ �� Ʈ���� �߰�

        //�����̼� ������Ʈ �ڵ��Ҵ� �޼��� �߰�
        //���� ������Ʈ, �ؽ�Ʈ �ڵ��Ҵ� �޼��� �߰�

        //�ش� ������Ʈ --> NextDialog() ȣ��
    }
    public void StartDialog(int dialogId)
    {
        if (dialogId != currentDialogId) return;  //������ ���� �۵�

        if (lastDialogNextId < 0)                 //�������� ���� ��, �������� �Ѿ
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

        lastDialogNextId = dialog.nextId;    //������ next ���̵�� �߰�

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
            dialogText.text = currentDialog.text;    //�����̼� �ؽ�Ʈ ����
        }

        //������ ǥ��
        ClearChoices();
    }
    private void CloseDialog()
    {
        dialogPanel.SetActive(false);
        currentDialog = null;
        StopTypingEffect();
    }

    //�ؽ�Ʈ Ÿ���� ȿ�� �ڷ�ƾ
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

    //Ÿ���� ȿ�� ����
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
        //���ñ۲���
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

    //������ ǥ��
    private void ShowChoices()
    {
        foreach (var choice in currentDialog.choices)
        {
            

            //������ �����ֱ�
        }
    }
}

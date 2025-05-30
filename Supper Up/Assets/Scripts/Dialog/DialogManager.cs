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
    [SerializeField] private List<TextMeshProUGUI> choiceTexts = new List<TextMeshProUGUI>();

    //[Header("TriggerObjects References")] //���� Ȥ�� �����̼� ��� ������Ʈ ����Ʈ
    //[SerializeField] List<GameObject> dialogTriggerObjects = new List<GameObject>();
    
    public List<GameObject> choiceObjects = new List<GameObject>();
    private List<int> selectList = new List<int>();

    [Header("Dialog Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypewriterEffect = true;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private DialogSO currentDialog;
    private int currentDialogId = 1;

    private int choiceCount = 0;
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

        ChoiceDialogObject[] choiceDialogObjects = FindObjectsOfType<ChoiceDialogObject>();
        var temps = new Dictionary<int, ChoiceDialogObject>();

        foreach (var obj in choiceDialogObjects)
            temps.Add(obj.choiceObjectId, obj);

        for (int i = 1; i <= choiceDialogObjects.Length; i++)
        {
            temps.TryGetValue(i, out  ChoiceDialogObject choiceDialogObject);
            choiceObjects.Add(choiceDialogObject.gameObject);
            choiceTexts.Add(choiceDialogObject.choiceText);
        }

        //���� �����̼��� ������ �� Ʈ���� �߰�

        //�����̼� ������Ʈ �ڵ��Ҵ� �޼��� �߰�
        //���� ������Ʈ, �ؽ�Ʈ �ڵ��Ҵ� �޼��� �߰�

        //�ش� ������Ʈ --> NextDialog() ȣ��
    }
    public void StartDialog(int dialogId)
    {
        if (dialogId != currentDialogId) return;  //������ ���� �۵�

        DialogSO dialog = dialogDatabase.GetDialogById(dialogId);
        if (dialog != null)
        {
            Debug.Log(currentDialogId);
            SoundManager.instance.PlaySound("Narration_" + currentDialogId);
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
        currentDialogId = currentDialog.nextId;
        dialogPanel.SetActive(true);

        ShowDialog();
        if (lastDialogNextId < 0)                 //�������� ���� ��, �������� �Ѿ
        {
            if (currentDialog.choices != null && currentDialog.choices.Count > 0)
            {
                ShowChoices();
            }
            return;
        }
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

    private void CloseDialog()
    {
        dialogPanel.SetActive(false);
        StopTypingEffect();
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

    public void ClearChoices()
    {
        //���ñ۲���
    }

    public void SelectChoice2(DialogChoiceSO choice)
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
            selectList.Add(choiceCount + 1);

            if (choiceTexts.Count > 0)
            {
                choiceTexts[choiceCount].text = choice.text;
                choiceCount++;
            }
            else
            {
                Debug.Log("�����ؽ�Ʈ�� �����ϴ�.");
            }

            //������ �����ֱ�
        }
    }

    public void SelectChoice(int id)
    {
        DialogChoiceSO choice = dialogDatabase.GetDialogChoiceByText(choiceTexts[id - 1].text);

        if (choice != null)
        {
            foreach (var num in selectList)    //���ô����, ���õ��� ���� ���� ����
            {
                if (id != num)
                {
                    var temp = choiceObjects.Find(p => p.GetComponent<ChoiceDialogObject>().choiceObjectId == num);
                    temp.GetComponent<ChoiceDialogObject>().isOneTime = true;
                }
            }

            switch (choice.choicePoint)
            {
                case 1: 
                    GameManager.Instance.choicePoint_1 += 1; 
                    Debug.Log("1�� ���ÿϷ�"); 
                break;
                case 2: 
                    GameManager.Instance.choicePoint_2 += 1; 
                    Debug.Log("2�� ���ÿϷ�"); 
                break;
            }

            currentDialogId = choice.nextId;
            StartDialog(choice.nextId);
        }
        else
        {
            Debug.Log("�������� �ʴ´�");
        }
    }
}

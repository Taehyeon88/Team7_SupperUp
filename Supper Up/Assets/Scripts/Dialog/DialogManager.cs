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

    //[Header("TriggerObjects References")] //선택 혹은 나레이션 대상 오브젝트 리스트
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

        //따로 나레이션이 나오게 할 트리거 추가

        //나레이션 오브젝트 자동할당 메서드 추가
        //선택 오브젝트, 텍스트 자동할당 메서드 추가

        //해당 오브젝트 --> NextDialog() 호출
    }
    public void StartDialog(int dialogId)
    {
        if (dialogId != currentDialogId) return;  //순서에 따라서 작동

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

        lastDialogNextId = dialog.nextId;    //마지막 next 아이디로 추가

        currentDialog = dialog;
        currentDialogId = currentDialog.nextId;
        dialogPanel.SetActive(true);

        ShowDialog();
        if (lastDialogNextId < 0)                 //선택지가 나올 시, 선택으로 넘어감
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
            dialogText.text = currentDialog.text;    //나레이션 텍스트 설정
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

    private void CloseDialog()
    {
        dialogPanel.SetActive(false);
        StopTypingEffect();
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

    public void ClearChoices()
    {
        //선택글끄기
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

    //선택지 표시
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
                Debug.Log("선택텍스트가 없습니다.");
            }

            //선택지 보여주기
        }
    }

    public void SelectChoice(int id)
    {
        DialogChoiceSO choice = dialogDatabase.GetDialogChoiceByText(choiceTexts[id - 1].text);

        if (choice != null)
        {
            foreach (var num in selectList)    //선택대상중, 선택되지 못한 발판 끄기
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
                    Debug.Log("1번 선택완료"); 
                break;
                case 2: 
                    GameManager.Instance.choicePoint_2 += 1; 
                    Debug.Log("2번 선택완료"); 
                break;
            }

            currentDialogId = choice.nextId;
            StartDialog(choice.nextId);
        }
        else
        {
            Debug.Log("존재하지 않는다");
        }
    }
}

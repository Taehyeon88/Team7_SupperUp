using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance {  get; private set; }

    [Header("Story References")]
    [SerializeField] private StoryDatebaseSO storyDatabase;

    [Header("UI References")]
    [SerializeField] private GameObject narrationPanel;
    [SerializeField] private TextMeshProUGUI narrationText;
    [SerializeField] private List<TextMeshProUGUI> choiceTexts = new List<TextMeshProUGUI>();
    public List<GameObject> choiceObjects = new List<GameObject>();

    [Header("Narration Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private bool useTypewriterEffect = true;

    [Header("EndGame Settings")]
    [SerializeField] private GameObject endGamePanal;
    [SerializeField] private TextMeshProUGUI endingText;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private List<int> selectList = new List<int>();
    private StoryNarrationSO currentNarration;
    private int currentStoryId = 1;

    private int choiceCount = 0;
    private int soundPlayId = 0;

    private bool playEventNarration = false;
    private int eventCount = 0;
    private string eventSoundPlayText;

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
        if (storyDatabase != null)
        {
            storyDatabase.Initialize();
        }
        else
        {
            Debug.LogError("Story Database is not assigned to Dialog Manager");
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
    }
    public void StartNarration(int storyId)
    {
        if (storyId != currentStoryId) return;                                  //Idüũ 

        StoryNarrationSO narration = storyDatabase.GetNarrationById(storyId);   //���� ���̵� �´� �����̼� ������ ã��
        if (narration != null)
        {
            //lastStoryNextId = narration.nId;                                     //���� ������ ���� �Ǵ� �̺�Ʈ ���� Ȯ�ο� Id�Ҵ�
            soundPlayId = narration.id;

            currentNarration = narration;                                           //���� �����̼� �����͸� �ֱ� �����̼����� �Ҵ�(�ʵ�� ���� ���ؼ�)
            currentStoryId = currentNarration.nextId;                               //�������� ����� Id �Ҵ�
            narrationPanel.SetActive(true);                                         //�����̼� �г� Ȱ��ȭ

            SoundManager.instance.PlaySound("Narration_" + soundPlayId);         //�ش� Id�� �´� �����̼� ����
            ShowStory(currentNarration.text);                                    //�����̼� Text����

            if (currentStoryId < 0 && currentStoryId >= -1)                       //�������� ���, ����
            {
                if (currentNarration.choices != null && currentNarration.choices.Count > 0)
                {
                    ShowChoices();
                }
            }
            else if (currentStoryId < -1)                                           //�̺�Ʈ�� ���, ����
            {
                if (currentNarration.events != null && currentNarration.events.Count > 0)
                {
                    if (eventCount > 3) return;

                    playEventNarration = true;
                    string eventText = "";

                    foreach (var s in GameManager.Instance.choiceds)
                    {
                        StoryEventSO data = currentNarration.events.Find(p => p.condition == s);

                        if (data == null) continue;
                        eventText = data.eventText;
                        eventSoundPlayText = data.condition;
                        currentStoryId = data.nextId;
                    }

                    ShowStory(eventText);                                           //�̺�Ʈ Text����
                }
            }
        }
        else
        {
            Debug.LogError($"Narration with ID {storyId} not found!");
        }
    }

    public void ShowStory(string text)
    {
        if (playEventNarration)
        {
            StartCoroutine(PauseWhileTyping(text));
            playEventNarration = false;
        }
        else
        {
            StartTypingEffect(text);
        }
    }
    private IEnumerator PauseWhileTyping(string text)
    {
        yield return new WaitUntil(() => !isTyping);
        narrationPanel.SetActive(true);                                           //�����̼� �г� Ȱ��ȭ
        SoundManager.instance.PlaySound("Narration_" + eventSoundPlayText);
        eventSoundPlayText = "";
        StartTypingEffect(text);
    }

    private void StartTypingEffect(string text)
    {
        Debug.Log(text);

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
        narrationText.text = "";
        foreach (char c in text)
        {
            narrationText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        if(playEventNarration) yield return new WaitForSeconds(3f);
        else yield return new WaitForSeconds(7f);

        CloseNarration();
        isTyping = false;
    }

    private void CloseNarration()
    {
        narrationPanel.SetActive(false);
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

    //������ ǥ��
    private void ShowChoices()
    {
        foreach (var choice in currentNarration.choices)
        {
            selectList.Add(choiceCount + 1);                 //���� �����̼ǿ� �Ҵ�Ǿ��ִ� ������ �ޱ�(�ʵ�� ����)
            if (choiceTexts.Count > 0)
            {
                choiceTexts[choiceCount].text = choice.text;  //�ش� ���ô��(����)�� ������ �ؽ�Ʈ �Ҵ�
                choiceCount++;
            }
            else
            {
                Debug.Log("���� �ؽ�Ʈ�� �����ϴ�.");
            }
        }
    }

    public void SelectChoice(int id)                        //������(���� �浹) ����
    {
        StoryChoiceSO choice = storyDatabase.GetStoryChoiceByText(choiceTexts[id - 1].text);   //���õ� ������ Id�� �����͸� ã�Ƽ� �ޱ�

        foreach (var num in selectList)
        {
            if (id != num)                             //���� ���� ��, ���õ��� ���� ���ǵ��� ���ñ�� ��Ȱ��ȭ��Ű��
            {
                var temp = choiceObjects.Find(p => p.GetComponent<ChoiceDialogObject>().choiceObjectId == num);
                temp.GetComponent<ChoiceDialogObject>().isOneTime = true;
            }
        }

        if (choice != null)
        {
            GameManager.Instance.choiceds.Add(choice.text);  //������ �ؽ�Ʈ ����

            if (choice.nextId > 0)
            {
                currentStoryId = choice.nextId;                  //������ ���� ������ Id��ȣ �Ҵ�
                StartNarration(choice.nextId);                      //������, �ش� �������� ���� �����̼� ����
            }
            else
            {
                currentStoryId = 13;
            }
        }
        else
        {
            Debug.Log("�ش� �������� ã�� �� �����ϴ�.");
        }
    }

    public void EndGame()
    {
        endGamePanal.SetActive(true);
        StoryEndingSO endingData = storyDatabase.GetEndingById(GameManager.Instance.CheckEnding());

        //�ش� ���� �ؽ�Ʈ ����
        if (endingData == null) return;
        string[] t_temp = endingData.text.Split("_");
        string _endingText = $"{t_temp[0]}-{t_temp[1]}";
        _endingText += "\n";
        _endingText += "\n";
        _endingText += t_temp[2];

        endingText.text = _endingText;


        //�ش� ���� �����̼� �۵�
        string i_temp = endingData.id.ToString();
        i_temp = i_temp.Substring(i_temp.Length - 1);
        int id = int.Parse(i_temp);

        Debug.Log(id);

        SoundManager.instance.PlaySound("Ending_" + id);
        //���� ���� �߰�

        //���� �ؽ�Ʈ�� ����Ϻ��� �����ϴ� ���� �߰�
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Story References")]
    [SerializeField] private StoryDatebaseSO storyDatabase;

    //���丮 ������
    public List<string> choiceds = new List<string>();    //���������� ���õ� ������ ���� ������
    public bool isGameEnd = false;

    //[Header("��Ƽ� ������ ���ư��� �ʹ�")]
    //public bool choice_1_1 = false;
    //[Header("�װ� ���� �ʴ�")]
    //public bool choice_1_2 = false;
    //[Header("���� ��縦 �� ���̴�")]
    //public bool choice_1_3 = false;
    //[Header("���� ì���")]
    //public bool choice_2_1 = false;
    //[Header("���� ì���� �ʴ´�")]
    //public bool choice_2_2 = false;
    //[Header("������ ì���")]
    //public bool choice_3_1 = false;
    //[Header("������ ì���� �ʴ´�")]
    //public bool choice_3_2 = false;
    //[Header("��� ���δ�")]
    //public bool choice_4_1 = false;
    //[Header("���������")]
    //public bool choice_4_2 = false;


    void Awake()
    {
       //Debug.Log("���ӽ��۵ȴ�");

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        isGameEnd = false;
    }

    private void Update()
    {
        if (choiceds.Count >= 4)
        {
            //Debug.Log(string.Join(",", choiceds));        
        
        }

        //if (choice_1_1) AddValueOneTime("��Ƽ� ������ ���ư��� �ʹ�");
        //if (choice_1_2) AddValueOneTime("�װ� ���� �ʴ�");
        //if (choice_1_3) AddValueOneTime("���� ��縦 �� ���̴�");
        //if (choice_2_1) AddValueOneTime("���� ì���");
        //if (choice_2_2) AddValueOneTime("���� ì���� �ʴ´�");
        //if (choice_3_1) AddValueOneTime("������ ì���");
        //if (choice_3_2) AddValueOneTime("������ ì���� �ʴ´�");
        //if (choice_4_1) AddValueOneTime("��� ���δ�");
        //if (choice_4_2) AddValueOneTime("���������");
    }

    //private void AddValueOneTime(string text)
    //{
    //    if (choiceds.Contains(text)) return;

    //    choiceds.Add(text);
    //}

    public int CheckEnding()
    {
        foreach (var ending in storyDatabase.storyEndings)
        {
            string[] conditions = ending.condition.Split('_');
            int metchingCount = conditions.Length;

            foreach (var choice in choiceds)
            {
                if (conditions.Contains(choice))
                {
                    metchingCount--;
                    //Debug.Log($"{metchingCount},{choice}");
                }
            }

            if (metchingCount <= 0) return ending.id;
        }
        return 0;
    }
}

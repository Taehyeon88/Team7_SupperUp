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

    //스토리 데이터
    public List<string> choiceds = new List<string>();    //선택지에서 선택된 선택지 누적 데이터
    public bool isGameEnd = false;

    //[Header("살아서 집으로 돌아가고 싶다")]
    //public bool choice_1_1 = false;
    //[Header("죽고 싶지 않다")]
    //public bool choice_1_2 = false;
    //[Header("나는 기사를 될 것이다")]
    //public bool choice_1_3 = false;
    //[Header("물을 챙긴다")]
    //public bool choice_2_1 = false;
    //[Header("물을 챙기지 않는다")]
    //public bool choice_2_2 = false;
    //[Header("보물을 챙긴다")]
    //public bool choice_3_1 = false;
    //[Header("보물을 챙기지 않는다")]
    //public bool choice_3_2 = false;
    //[Header("모두 죽인다")]
    //public bool choice_4_1 = false;
    //[Header("살려보낸다")]
    //public bool choice_4_2 = false;


    void Awake()
    {
       //Debug.Log("게임시작된다");

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

        //if (choice_1_1) AddValueOneTime("살아서 집으로 돌아가고 싶다");
        //if (choice_1_2) AddValueOneTime("죽고 싶지 않다");
        //if (choice_1_3) AddValueOneTime("나는 기사를 될 것이다");
        //if (choice_2_1) AddValueOneTime("물을 챙긴다");
        //if (choice_2_2) AddValueOneTime("물을 챙기지 않는다");
        //if (choice_3_1) AddValueOneTime("보물을 챙긴다");
        //if (choice_3_2) AddValueOneTime("보물을 챙기지 않는다");
        //if (choice_4_1) AddValueOneTime("모두 죽인다");
        //if (choice_4_2) AddValueOneTime("살려보낸다");
    }

    private void AddValueOneTime(string text)
    {
        if (choiceds.Contains(text)) return;

        choiceds.Add(text);
    }
    //살아서 집으로 돌아가고 싶다_모두 죽인다_보물을 챙기지 않는다
    //살아서 집으로 돌아가고 싶다_모두 죽인다_보물을 챙긴다
    //살아서 집으로 돌아가고 싶다_살려보낸다_물을 챙기지 않는다
    //살아서 집으로 돌아가고 싶다_살려보낸다_물을 챙긴다
    //죽고 싶지 않다
    //나는 기사를 될 것이다_보물을 챙기지 않는다
    //나는 기사를 될 것이다_보물을 챙긴다

    public void EndingCheat(int num)
    {
        switch (num)
        {
            case 1:
                AddValueOneTime("살아서 집으로 돌아가고 싶다");
                AddValueOneTime("모두 죽인다");
                AddValueOneTime("보물을 챙기지 않는다");
                AddValueOneTime("물을 챙기지 않는다");
                break;
            case 2:
                AddValueOneTime("살아서 집으로 돌아가고 싶다");
                AddValueOneTime("모두 죽인다");
                AddValueOneTime("보물을 챙긴다");
                AddValueOneTime("물을 챙기지 않는다");
                break;
            case 3:
                AddValueOneTime("살아서 집으로 돌아가고 싶다");
                AddValueOneTime("살려보낸다");
                AddValueOneTime("보물을 챙긴다");
                AddValueOneTime("물을 챙기지 않는다");
                break;
            case 4:
                AddValueOneTime("살아서 집으로 돌아가고 싶다");
                AddValueOneTime("살려보낸다");
                AddValueOneTime("보물을 챙긴다");
                AddValueOneTime("물을 챙긴다");
                break;
            case 5:
                AddValueOneTime("죽고 싶지 않다");
                AddValueOneTime("살려보낸다");
                AddValueOneTime("보물을 챙긴다");
                AddValueOneTime("물을 챙긴다");
                break;
            case 6:
                AddValueOneTime("나는 기사를 될 것이다");
                AddValueOneTime("살려보낸다");
                AddValueOneTime("보물을 챙긴다");
                AddValueOneTime("물을 챙긴다");
                break;
            case 7:
                AddValueOneTime("나는 기사를 될 것이다");
                AddValueOneTime("살려보낸다");
                AddValueOneTime("보물을 챙기지 않는다");
                AddValueOneTime("물을 챙긴다");
                break;
        }
    }

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

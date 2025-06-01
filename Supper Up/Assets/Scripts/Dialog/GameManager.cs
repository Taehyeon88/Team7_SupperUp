using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //스토리 데이터
    public List<string> choiceds = new List<string>();    //선택지에서 선택된 선택지 누적 데이터

    void Awake()
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
    }

    private void Update()
    {
        if (choiceds.Count >= 4)
        {
            Debug.Log(string.Join(",", choiceds));        
        
        }
    }
}

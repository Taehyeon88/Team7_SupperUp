using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //���丮 ������
    public List<string> choiceds = new List<string>();    //���������� ���õ� ������ ���� ������

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

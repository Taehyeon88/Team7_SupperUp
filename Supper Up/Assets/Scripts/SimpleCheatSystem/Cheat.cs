using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public GameObject player;
    public List<Transform> trans = new List<Transform>();

    private int num;

    public Transform movetrans;
    void Start()
    {
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    num++;
        //    player.transform.position = trans[num].position;
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    num--;
        //    player.transform.position = trans[num].position;
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    player.transform.position = trans[num].position;
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(2);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(3);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(4);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(5);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(6);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    player.transform.position = movetrans.position;
        //    GameManager.Instance.EndingCheat(7);
        //}
    }
}

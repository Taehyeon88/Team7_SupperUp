using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public GameObject player;
    public List<Transform> trans = new List<Transform>();

    private int num;
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
    }
}

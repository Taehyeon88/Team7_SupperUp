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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            num++;
            player.transform.position = trans[num].position;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            num--;
            player.transform.position = trans[num].position;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.transform.position = trans[num].position;
        }
    }
}

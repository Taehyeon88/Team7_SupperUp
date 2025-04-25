using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    //UD_Floor
    float initPositionY;
    float initPositionX;
    public float distance;
    public float turningPoint;
    //UD_Floor & LR_Floor
    public bool turnSwitch;
    public float moveSpeed;

    //RT_Floor
    public float rotateSpeed;

    void Awake()
    {
        if (gameObject.name == "UD_Floor")
        {
            initPositionY = transform.position.y;
            turningPoint = initPositionY - distance;
        }
        if (gameObject.name == "LR_Floor")
        {
            initPositionX = transform.position.x;
            turningPoint = initPositionX - distance;
        }
    }

    void upDown()
    {
        float currentPositionY = transform.position.y;

        if (currentPositionY >= initPositionY)
        {
            turnSwitch = false;
        }
        else if (currentPositionY <= turningPoint)
        {
            turnSwitch = true;
        }

        if (turnSwitch)
        {
            transform.position = transform.position + new Vector3(0, 1, 0) * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + new Vector3(0, -1, 0) * moveSpeed * Time.deltaTime;
        }
    }
    void rotate()
    {
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);
    }
    void leftRight()
    {

        float currentPositionX = transform.position.x;

        if (currentPositionX >= initPositionX + distance)
        {
            turnSwitch = false;
        }
        else if (currentPositionX <= turningPoint)
        {
            turnSwitch = true;
        }

        if (turnSwitch)
        {
            transform.position = transform.position + new Vector3(1, 0, 0) * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position + new Vector3(-1, 0, 0) * moveSpeed * Time.deltaTime;
        }

    }

    void Update()
    {
        if (gameObject.name == "UD_Floor")
        {
            upDown();
        }
        if (gameObject.name == "RT_Floor")
        {
            rotate();
        }
        if (gameObject.name == "LR_Floor")
        {
            leftRight();
        }

    }
}
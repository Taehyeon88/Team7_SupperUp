using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    float initPositionY;
    float initPositionX;
    public float distance;
    public float turningPoint;

    public bool turnSwitch;
    public float moveSpeed;

    public float rotateSpeed;

    void Awake()
    {
        if (gameObject.name == "UD_Floor")
        {
            initPositionY = transform.position.y;
            turningPoint = initPositionY - distance;
            turnSwitch = true; // 초기 상태
        }
        if (gameObject.name == "LR_Floor")
        {
            initPositionX = transform.position.x;
            turningPoint = initPositionX - distance;
            turnSwitch = true; // 초기 상태
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
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        else
        {
            transform.position += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
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
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
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

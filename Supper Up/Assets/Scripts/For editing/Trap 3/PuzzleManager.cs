using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject bridgePrefab; // 다리 프리팹
    public Vector3 bridgeSpawnPosition; // 다리가 생성될 위치
    public Color initialColor = Color.white;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;

    private List<int> correctSequence = new List<int> { 1, 2, 5, 6, 9 };
    private List<int> playerSequence = new List<int>();
    private bool puzzleSolved = false;

    void Start()
    {
        // 시작할 때는 다리가 없습니다.
    }

    public void StepOnPlatform(int platformIndex)
    {
        if (puzzleSolved) return;

        Debug.Log($"Stepped on platform {platformIndex}");

        playerSequence.Add(platformIndex);
        platforms[platformIndex - 1].GetComponent<Renderer>().material.color = correctColor;

        CheckSequence();
    }

    void CheckSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            if (playerSequence.Count == correctSequence.Count)
            {
                PuzzleSolved();
            }
        }
        else
        {
            StartCoroutine(ResetPlatforms());
        }
    }

    void PuzzleSolved()
    {
        puzzleSolved = true;
        Debug.Log("Puzzle solved! Creating the bridge.");
        CreateBridge();
    }

    void CreateBridge()
    {
        if (bridgePrefab != null)
        {
            GameObject bridge = Instantiate(bridgePrefab, bridgeSpawnPosition, Quaternion.identity);
            Debug.Log("Bridge created successfully.");
        }
        else
        {
            Debug.LogError("Bridge prefab is not assigned in the inspector!");
        }
    }

    IEnumerator ResetPlatforms()
    {
        Debug.Log("Resetting platforms");
        foreach (GameObject platform in platforms)
        {
            platform.GetComponent<Renderer>().material.color = wrongColor;
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject platform in platforms)
        {
            platform.GetComponent<Renderer>().material.color = initialColor;
        }

        playerSequence.Clear();
    }
}

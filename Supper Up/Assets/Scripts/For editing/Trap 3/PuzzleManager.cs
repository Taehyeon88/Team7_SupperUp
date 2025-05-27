using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public PressurePlate[] plates;
    public GameObject bridge;
    private List<int> correctSequence = new List<int> { 1, 2, 3, 8, 7, 12, 13, 14, 19, 18, 17, 22, 23, 24, 25 };
    private List<int> playerSequence = new List<int>();

    public void ActivatePlate(int plateNumber)
    {
        playerSequence.Add(plateNumber);
        CheckSequence();
    }

    private void CheckSequence()
    {
        bool isCorrect = true;
        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (i >= correctSequence.Count || playerSequence[i] != correctSequence[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            if (playerSequence.Count == correctSequence.Count)
            {
                CompletePuzzle();
            }
            plates[playerSequence[playerSequence.Count - 1] - 1].SetColor(Color.yellow);
        }
        else
        {
            FailPuzzle();
        }
    }

    private void CompletePuzzle()
    {
        Debug.Log("∆€¡Ò øœ∑·!");
        bridge.SetActive(true);
    }

    private void FailPuzzle()
    {
        Debug.Log("∆€¡Ò Ω«∆–!");
        foreach (int plateNumber in playerSequence)
        {
            plates[plateNumber - 1].SetColor(Color.red);
        }
        playerSequence.Clear();
    }
}
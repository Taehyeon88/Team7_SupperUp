using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    public int platformIndex;
    private PuzzleManager puzzleManager;

    void Start()
    {
        puzzleManager = FindObjectOfType<PuzzleManager>();
        if (puzzleManager == null)
        {
            Debug.LogError("PuzzleManager not found in the scene!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log($"Player collided with platform {platformIndex}");
            if (puzzleManager != null)
            {
                puzzleManager.StepOnPlatform(platformIndex);
            }
            else
            {
                Debug.LogError("PuzzleManager is null!");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public int plateNumber;
    public PuzzleManager puzzleManager;
    private Renderer plateRenderer;

    private void Start()
    {
        plateRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            puzzleManager.ActivatePlate(plateNumber);
            SetColor(Color.green);
        }
    }

    public void SetColor(Color color)
    {
        plateRenderer.material.color = color;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationObject : MonoBehaviour
{
    [SerializeField] private int id;

    private bool isOneTime = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOneTime)
        {
            if (StoryManager.Instance != null)
            {
                StoryManager.Instance.StartNarration(id);
                Debug.Log("OK");
            }
            isOneTime = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingObject : MonoBehaviour
{
    private bool isOneTime = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOneTime)
        {
            if (StoryManager.Instance != null)
            {
                StoryManager.Instance.EndGame();
                Debug.Log("End");
            }

            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.choiceds.Count >= 4)
                    isOneTime = true;
            }

            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound("GameEnd");
            }
        }
    }
}

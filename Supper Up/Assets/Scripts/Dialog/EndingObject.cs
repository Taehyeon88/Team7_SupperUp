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

            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.choiceds.Count < 4) return;

                isOneTime = true;
                Cursor.lockState = CursorLockMode.None;

                GameManager.Instance.isGameEnd = true;

                if (StoryManager.Instance != null)
                {
                    StoryManager.Instance.EndGame();
                    Debug.Log("End");
                }

                if (SoundManager.instance != null)
                {
                    SoundManager.instance.PauseAllSounds();
                    SoundManager.instance.PlaySound("GameEnd");
                }

                //Time.timeScale = 0f;
            }
        }
    }
}

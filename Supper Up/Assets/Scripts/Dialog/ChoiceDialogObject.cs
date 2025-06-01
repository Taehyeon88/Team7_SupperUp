using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceDialogObject : MonoBehaviour
{
    public int choiceObjectId;

    public TextMeshProUGUI choiceText;

    public bool isOneTime = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !isOneTime)
        {
            if (StoryManager.Instance != null)
            {
                StoryManager.Instance.SelectChoice(choiceObjectId);
            }
            isOneTime = true;
        }
    }
}

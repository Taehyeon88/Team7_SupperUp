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
            if (DialogManager.Instance != null)
            {
                DialogManager.Instance.SelectChoice(choiceObjectId);
            }
            isOneTime = true;
        }
    }
}

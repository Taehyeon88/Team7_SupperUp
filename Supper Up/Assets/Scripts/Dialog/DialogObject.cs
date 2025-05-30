using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogObject : MonoBehaviour
{
    [SerializeField] private int id;

    private bool isOneTime = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOneTime)
        {
            if (DialogManager.Instance != null)
            {
                DialogManager.Instance.StartDialog(id);
            }
            isOneTime = true;
        }
    }
}

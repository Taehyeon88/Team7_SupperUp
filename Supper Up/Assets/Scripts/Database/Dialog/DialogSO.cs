using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog System/Dialog")]
public class DialogSO : ScriptableObject
{
    public int id;
    public string text;
    public int nextId;
    public List<DialogChoiceSO> choices = new List<DialogChoiceSO>();
}

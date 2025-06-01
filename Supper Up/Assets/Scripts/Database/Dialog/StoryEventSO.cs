using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story Event", menuName = "Story System/Story Event")]
public class StoryEventSO : ScriptableObject
{
    public string condition;
    public string eventText;
    public int nextId;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story Choice", menuName = "Story System/Story Choice")]

public class StoryChoiceSO : ScriptableObject
{
    public string text;
    public int nextId;
}

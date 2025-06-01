using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryNarration", menuName = "Story System/Narration")]
public class StoryNarrationSO : ScriptableObject
{
    public int id;
    public string text;
    public int nextId;
    public List<StoryChoiceSO> choices = new List<StoryChoiceSO>();
    public List<StoryEventSO> events = new List<StoryEventSO>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story Ending", menuName = "Story System/Story Ending")]
public class StoryEndingSO : ScriptableObject
{
    public int id;
    public string text;
    public string condition;
}

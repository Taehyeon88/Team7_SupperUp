using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryDatebase", menuName = "Story System/Datebase")]
public class StoryDatebaseSO : ScriptableObject
{
    public List<StoryNarrationSO> narrations = new List<StoryNarrationSO>();
    public List<StoryChoiceSO> storyChoices = new List<StoryChoiceSO>();
    public List<StoryEventSO> storyEvents = new List<StoryEventSO>();
    public List<StoryEndingSO> storyEndings = new List<StoryEndingSO>();

    private Dictionary<int, StoryNarrationSO> narrationsById;
    private Dictionary<string, StoryChoiceSO> StorychoicesByText;

    public void Initialize()
    {
        narrationsById = new Dictionary<int, StoryNarrationSO>();
        StorychoicesByText = new Dictionary<string, StoryChoiceSO>();

        foreach (var dialog in narrations)
        {
            if (dialog != null)
            {
                narrationsById[dialog.id] = dialog;
            }
        }
        
        foreach (var choice in storyChoices)
        {
            if (choice != null)
            {
                StorychoicesByText[choice.text] = choice;
            }
        }
    }

    public StoryNarrationSO GetNarrationById(int id)
    {
        if (narrationsById == null)
            Initialize();

        if(narrationsById.TryGetValue(id, out StoryNarrationSO dialog))
            return dialog;

        return null;
    }

    public StoryChoiceSO GetStoryChoiceByText(string text)
    {
        if (StorychoicesByText == null)
            Initialize();

        if (StorychoicesByText.TryGetValue(text, out StoryChoiceSO choice))
            return choice;

        return null;
    }
}

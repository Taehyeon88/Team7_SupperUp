using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogDatebase", menuName = "Dialog System/Datebase")]
public class DialogDatebaseSO : ScriptableObject
{
    public List<DialogSO> dialogs = new List<DialogSO>();
    public List<DialogChoiceSO> choiceDialogs = new List<DialogChoiceSO>();

    private Dictionary<int, DialogSO> dialogsById;
    private Dictionary<string, DialogChoiceSO> choiceDialogsByText;

    public void Initialize()
    {
        dialogsById = new Dictionary<int, DialogSO>();
        choiceDialogsByText = new Dictionary<string, DialogChoiceSO>();

        foreach (var dialog in dialogs)
        {
            if (dialog != null)
            {
                dialogsById[dialog.id] = dialog;
            }
        }
        
        foreach (var choice in choiceDialogs)
        {
            if (choice != null)
            {
                choiceDialogsByText[choice.text] = choice;
            }
        }
    }

    public DialogSO GetDialogById(int id)
    {
        if (dialogsById == null)
            Initialize();

        if(dialogsById.TryGetValue(id, out DialogSO dialog))
            return dialog;

        return null;
    }

    public DialogChoiceSO GetDialogChoiceByText(string text)
    {
        if (choiceDialogsByText == null)
            Initialize();

        if (choiceDialogsByText.TryGetValue(text, out DialogChoiceSO choice))
            return choice;

        return null;
    }
}

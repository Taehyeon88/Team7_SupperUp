#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class DialogRowData
{
    public int? id;
    public string text;
    public int? nextId;
    public string choiceText;
    public int? choiceNextId;
    public int? choicePoint;
}

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                //JSON ���� ��� ���ڿ� ��
    private string outputFolder = "Assets/ScriptableObjects/Dialogs";        //��� SO ���� ��ΰ�
    private bool createDatabase = true;

    [MenuItem("Tools/JSON to Scriptable Objects")]
    public static void ShowWindow()
    {
        GetWindow<JsonToScriptableConverter>("JSON to Scriptable Objects");
    }
    void OnGUI()
    {
        GUILayout.Label("JSON to Scriptable Object Converter", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        if (GUILayout.Button("Select JSON File"))
        {
            jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", "", "json");
        }

        EditorGUILayout.LabelField("Selected File: ", jsonFilePath);
        EditorGUILayout.Space();
        outputFolder = EditorGUILayout.TextField("Output Folder: ", outputFolder);
        createDatabase = EditorGUILayout.Toggle("Create Database Asset", createDatabase);
        EditorGUILayout.Space();

        if (GUILayout.Button("Convert to Scriptable Objects"))
        {
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                EditorUtility.DisplayDialog("Error", "Please sellect a JSON file firest!", "OK");
                return;
            }
        }
    }

    private void ConvertJsonToDialogScriptableObjects()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        string jsonText = File.ReadAllText(jsonFilePath);

        try
        {
            List<DialogRowData> rowDataList = JsonConvert.DeserializeObject<List<DialogRowData>>(jsonText);

            Dictionary<int, DialogSO> dialogMap = new Dictionary<int, DialogSO>();
            List<DialogSO> createDialogs = new List<DialogSO>();

            //1�ܰ� : ��ȭ �׸� ����
            foreach (var rowData in rowDataList)
            {
                if (rowData.id.HasValue)
                {
                    DialogSO dialogSO = ScriptableObject.CreateInstance<DialogSO>();

                    dialogSO.id = rowData.id.Value;
                    dialogSO.text = rowData.text;
                    dialogSO.nextId = rowData.nextId.HasValue ? rowData.nextId.Value : -1;
                    dialogSO.choices = new List<DialogChoiceSO>();

                    dialogMap[dialogSO.id] = dialogSO;
                    createDialogs.Add(dialogSO);
                }
            }
            //2�ܰ� : ������ �׸� ó�� �� ����
            foreach (var rowData in rowDataList)
            {
                if (!rowData.id.HasValue && !string.IsNullOrEmpty(rowData.choiceText) && rowData.choiceNextId.HasValue && rowData.choicePoint.HasValue)
                {
                    int parentId = -1; //���� ���� ID�� �θ�ID�� ���

                    //�� ������ �ٷ� ���� �ִ� ��ȭ(id�� �ִ� �׸�)�� ã��
                    int currentIndex = rowDataList.IndexOf(rowData);
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (rowDataList[i].id.HasValue)
                        {
                            parentId = rowDataList[i].id.Value;
                            break;
                        }
                    }

                    //�θ� ID�� ã�� ���߰ų� �θ� ID�� -1�� ��� (�� ��° �׸�)
                    if (parentId == -1)
                    {
                        Debug.LogWarning($"������ '{rowData.choiceText}'�� �θ� ��ȭ�� ã�� �� �����ϴ�.");
                    }

                    if (dialogMap.TryGetValue(parentId, out DialogSO parentDialog))
                    {
                        DialogChoiceSO choiceSO = ScriptableObject.CreateInstance<DialogChoiceSO>();
                        choiceSO.text = rowData.choiceText;
                        choiceSO.nextId = rowData.choiceNextId.Value;
                        choiceSO.choicePoint = rowData.choicePoint.Value;

                        //������ ���� ����
                        string choiceAssetPath = $"{outputFolder}/Choice_{parentId}_{parentDialog.choices.Count + 1}.asset";
                        EditorUtility.SetDirty(choiceSO);

                        parentDialog.choices.Add(choiceSO);
                    }
                    else
                    {
                        Debug.Log($"������ '{rowData.choiceText}'�� ������ ��ȭ (ID : {parentId})�� ã�� �� �����ϴ�.");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to convert JSON: {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ���� : {e} ");
        }
    }

}

#endif

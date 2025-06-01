#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class StoryRowData
{
    public int? id;
    public string text;
    public int? nextId;
    public string choiceText;
    public int? choiceNextId;
    public string eventCondition;
    public string eventText;
    public int? eventNextId;
}

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                //JSON ���� ��� ���ڿ� ��
    private string outputFolder = "Assets/ScriptableObjects/Story";        //��� SO ���� ��ΰ�
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

            ConvertJsonToDialogScriptableObjects();
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
            List<StoryRowData> rowDataList = JsonConvert.DeserializeObject<List<StoryRowData>>(jsonText);

            Dictionary<int, StoryNarrationSO> NarrationMap = new Dictionary<int, StoryNarrationSO>();
            List<StoryNarrationSO> createNarrations = new List<StoryNarrationSO>();
            List<StoryChoiceSO> createStoryChoices = new List<StoryChoiceSO>();
            List<StoryEventSO> createStoryEvents = new List<StoryEventSO>();
            List<StoryEndingSO> createEndings = new List<StoryEndingSO>();

            //1�ܰ� : �����̼� �׸� ����
            foreach (var rowData in rowDataList)
            {
                if (rowData.id.HasValue && rowData.nextId.HasValue)  //id�� nextId�� �־�� �Ѵ� <-- �����̼�����
                {
                    StoryNarrationSO NarrationSO = ScriptableObject.CreateInstance<StoryNarrationSO>();

                    NarrationSO.id = rowData.id.Value;
                    NarrationSO.text = rowData.text;
                    NarrationSO.nextId = rowData.nextId.HasValue ? rowData.nextId.Value : -1;
                    NarrationSO.choices = new List<StoryChoiceSO>();
                    NarrationSO.events = new List<StoryEventSO>();

                    NarrationMap[NarrationSO.id] = NarrationSO;
                    createNarrations.Add(NarrationSO);
                }
                else if(rowData.id.HasValue && !string.IsNullOrEmpty(rowData.eventCondition))
                {
                    StoryEndingSO endingSO = ScriptableObject.CreateInstance<StoryEndingSO>();

                    endingSO.id = rowData.id.Value;
                    endingSO.text = rowData.text;
                    endingSO.condition = rowData.eventCondition;

                    createEndings.Add(endingSO);
                }
            }
            //2�ܰ� : ������ �׸� ó�� �� ���� + �̺�Ʈ �׸� ó�� �� ����
            foreach (var rowData in rowDataList)
            {
                if (!rowData.id.HasValue)
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

                    if (NarrationMap.TryGetValue(parentId, out StoryNarrationSO parentDialog))
                    {
                        if (!string.IsNullOrEmpty(rowData.choiceText) && rowData.choiceNextId.HasValue)   //����������
                        {
                            StoryChoiceSO choiceSO = ScriptableObject.CreateInstance<StoryChoiceSO>();
                            choiceSO.text = rowData.choiceText;
                            choiceSO.nextId = rowData.choiceNextId.Value;

                            //������ ���� ����
                            string choiceAssetPath = $"{outputFolder}/Choice_{parentId}_{parentDialog.choices.Count + 1}.asset";
                            AssetDatabase.CreateAsset(choiceSO, choiceAssetPath);
                            EditorUtility.SetDirty(choiceSO);

                            parentDialog.choices.Add(choiceSO);
                            createStoryChoices.Add(choiceSO);
                        }
                        else if (!string.IsNullOrEmpty(rowData.eventCondition) && !string.IsNullOrEmpty(rowData.eventText) && rowData.eventNextId.HasValue)  //�̺�Ʈ����
                        {
                            StoryEventSO eventSO = ScriptableObject.CreateInstance<StoryEventSO>();
                            eventSO.eventText = rowData.eventText;
                            eventSO.condition = rowData.eventCondition;
                            eventSO.nextId = rowData.eventNextId.Value;

                            //������ ���� ����
                            string eventAssetPath = $"{outputFolder}/Event_{parentId}_{parentDialog.events.Count + 1}.asset";
                            AssetDatabase.CreateAsset(eventSO, eventAssetPath);
                            EditorUtility.SetDirty(eventSO);

                            parentDialog.events.Add(eventSO);
                            createStoryEvents.Add(eventSO);
                        }
                    }
                    else
                    {
                        Debug.Log($"������ '{rowData.choiceText}'�� ������ ��ȭ (ID : {parentId})�� ã�� �� �����ϴ�.");
                    }
                }
            }

            //3�ܰ� : �����̼� ��ũ���ͺ� ������Ʈ ����
            foreach (var narration in createNarrations)
            {
                //��ũ���ͺ� ������Ʈ ���� - ID�� 4�ڸ� ���ڷ� ������
                string assetPath = $"{outputFolder}/Narration_{narration.id.ToString("D4")}.asset";
                AssetDatabase.CreateAsset( narration, assetPath );

                //���� �̸� ����
                narration.name = $"Narration_{narration.id.ToString("D4")}";

                EditorUtility.SetDirty(narration);
            }

            foreach (var ending in createEndings)
            {
                string assetPath = $"{outputFolder}/Ending_{ending.id.ToString("D4")}.asset";
                AssetDatabase.CreateAsset(ending, assetPath);

                ending.name = $"Ending_{ending.id.ToString("D4")}";

                EditorUtility.SetDirty(ending);
            }

            if (createDatabase && createNarrations.Count > 0 && createStoryChoices.Count > 0)
            {
                StoryDatebaseSO database = ScriptableObject.CreateInstance<StoryDatebaseSO>();
                database.narrations = createNarrations;
                database.storyChoices = createStoryChoices;
                database.storyEvents = createStoryEvents;
                database.storyEndings = createEndings;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/StoryDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Success", $"Created {createNarrations.Count} Story scriptable objects!", "Ok");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to convert JSON: {e.Message}", "OK");
            Debug.LogError($"JSON ��ȯ ���� : {e} ");
        }
    }

}

#endif

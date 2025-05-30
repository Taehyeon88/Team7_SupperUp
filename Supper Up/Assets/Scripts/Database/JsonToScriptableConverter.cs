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
    private string jsonFilePath = "";                                //JSON 파일 경로 문자열 값
    private string outputFolder = "Assets/ScriptableObjects/Dialogs";        //출력 SO 파일 경로값
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
            List<DialogRowData> rowDataList = JsonConvert.DeserializeObject<List<DialogRowData>>(jsonText);

            Dictionary<int, DialogSO> dialogMap = new Dictionary<int, DialogSO>();
            List<DialogSO> createDialogs = new List<DialogSO>();
            List<DialogChoiceSO> createDialogChoices = new List<DialogChoiceSO>();

            //1단계 : 대화 항목 생성
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
            //2단계 : 선택지 항목 처리 및 연결
            foreach (var rowData in rowDataList)
            {
                if (!rowData.id.HasValue && !string.IsNullOrEmpty(rowData.choiceText) && rowData.choiceNextId.HasValue && rowData.choicePoint.HasValue)
                {
                    int parentId = -1; //이전 행의 ID를 부모ID로 사용

                    //이 선택지 바로 위에 있는 대화(id가 있는 항목)을 찾음
                    int currentIndex = rowDataList.IndexOf(rowData);
                    for (int i = currentIndex - 1; i >= 0; i--)
                    {
                        if (rowDataList[i].id.HasValue)
                        {
                            parentId = rowDataList[i].id.Value;
                            break;
                        }
                    }

                    //부모 ID를 찾지 못했거나 부모 ID가 -1인 경우 (쳣 번째 항목)
                    if (parentId == -1)
                    {
                        Debug.LogWarning($"선택지 '{rowData.choiceText}'의 부모 대화를 찾을 수 없습니다.");
                    }

                    if (dialogMap.TryGetValue(parentId, out DialogSO parentDialog))
                    {
                        DialogChoiceSO choiceSO = ScriptableObject.CreateInstance<DialogChoiceSO>();
                        choiceSO.text = rowData.choiceText;
                        choiceSO.nextId = rowData.choiceNextId.Value;
                        choiceSO.choicePoint = rowData.choicePoint.Value;

                        //선택지 에셋 저장
                        string choiceAssetPath = $"{outputFolder}/Choice_{parentId}_{parentDialog.choices.Count + 1}.asset";
                        AssetDatabase.CreateAsset(choiceSO, choiceAssetPath);
                        EditorUtility.SetDirty(choiceSO);

                        parentDialog.choices.Add(choiceSO);
                        createDialogChoices.Add(choiceSO);
                    }
                    else
                    {
                        Debug.Log($"선택지 '{rowData.choiceText}'를 연결할 대화 (ID : {parentId})를 찾을 수 없습니다.");
                    }
                }
            }

            //3단계 : 대화 스크립터블 오브젝트 저장
            foreach (var dialog in createDialogs)
            {
                //스크립터블 오브젝트 저장 - ID를 4자리 숫자로 포멧팅
                string assetPath = $"{outputFolder}/Dialog_{dialog.id.ToString("D4")}.asset";
                AssetDatabase.CreateAsset( dialog, assetPath );

                //에셋 이름 저장
                dialog.name = $"Dialog_{dialog.id.ToString("D4")}";

                EditorUtility.SetDirty(dialog);
            }

            if (createDatabase && createDialogs.Count > 0 && createDialogChoices.Count > 0)
            {
                DialogDatebaseSO database = ScriptableObject.CreateInstance<DialogDatebaseSO>();
                database.dialogs = createDialogs;
                database.choiceDialogs = createDialogChoices;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/DialogDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Success", $"Created {createDialogs.Count} dialog scriptable objects!", "Ok");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to convert JSON: {e.Message}", "OK");
            Debug.LogError($"JSON 변환 오류 : {e} ");
        }
    }

}

#endif

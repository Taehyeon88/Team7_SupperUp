#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class JsonToScriptableConverter : EditorWindow
{
    private string jsonFilePath = "";                                //JSON 파일 경로 문자열 값
    private string outputFolder = "Assets/Resources/ScriptableObjects/sounds"; //출력 SO 파일 경로값
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
            ConvertJsonToScriptableObjects();
        }
    }

    private void ConvertJsonToScriptableObjects()
    {
        //폴더 생성
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        //JSON 파일 읽
        string jsonText = File.ReadAllText(jsonFilePath);  //Json 파일을 읽는다.

        try
        {
            //JSON 파싱
            List<SoundData> soundDataList = JsonConvert.DeserializeObject<List<SoundData>>(jsonText);

            List<SoundSO> createdSounds = new List<SoundSO>();

            //각 아이템 데이터를 스크립터를 오브젝트로 변환
            foreach (var soundData in soundDataList)
            {
                SoundSO soundSO = ScriptableObject.CreateInstance<SoundSO>();

                //데이터 복사
                soundSO.id = soundData.id;
                soundSO.pitch = soundData.pitch;
                soundSO.volume = soundData.volume;
                soundSO.loop = soundData.loop;
                soundSO.is3D = soundData.is3D;
                soundSO.maxDistance = soundData.maxDistance;

                //열거형 변환
                if (System.Enum.TryParse(soundData.soundTypeString, out SoundType parsedtype))
                {
                    soundSO.soundType = parsedtype;
                }
                else
                {
                    Debug.LogWarning($"사운드 '{soundData.id}'의 유허하지 않은 타입: {soundData.soundTypeString}");
                }
                soundSO.soundObjectType = soundData.soundObjectType;

                //스크립터블 오브젝트 저장
                string assetPath = $"{outputFolder}/Sound_{soundData.id}.asset";
                AssetDatabase.CreateAsset(soundSO, assetPath);

                //에셋이르 지정
                soundSO.name = $"sound_{soundData.id}";
                createdSounds.Add(soundSO);

                EditorUtility.SetDirty(soundSO);
            }

            //데이터베이스 생성
            if (createDatabase && createdSounds.Count > 0)
            {
                SoundDatabaseSO database = ScriptableObject.CreateInstance<SoundDatabaseSO>();
                database.sounds = createdSounds;

                AssetDatabase.CreateAsset(database, $"{outputFolder}/SoundDatabase.asset");
                EditorUtility.SetDirty(database);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Sucess", $"Created {createdSounds.Count} scriptalbe objects!", "OK");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Convert JSON: {e.Message}", "OK");
            Debug.LogError($"JSON 변환 오류: {e}");
        }
    }

}

#endif

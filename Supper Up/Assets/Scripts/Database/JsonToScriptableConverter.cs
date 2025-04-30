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
    private string jsonFilePath = "";                                //JSON ���� ��� ���ڿ� ��
    private string outputFolder = "Assets/Resources/ScriptableObjects/sounds"; //��� SO ���� ��ΰ�
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
        //���� ����
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        //JSON ���� ��
        string jsonText = File.ReadAllText(jsonFilePath);  //Json ������ �д´�.

        try
        {
            //JSON �Ľ�
            List<SoundData> soundDataList = JsonConvert.DeserializeObject<List<SoundData>>(jsonText);

            List<SoundSO> createdSounds = new List<SoundSO>();

            //�� ������ �����͸� ��ũ���͸� ������Ʈ�� ��ȯ
            foreach (var soundData in soundDataList)
            {
                SoundSO soundSO = ScriptableObject.CreateInstance<SoundSO>();

                //������ ����
                soundSO.id = soundData.id;
                soundSO.pitch = soundData.pitch;
                soundSO.volume = soundData.volume;
                soundSO.loop = soundData.loop;
                soundSO.is3D = soundData.is3D;
                soundSO.maxDistance = soundData.maxDistance;

                //������ ��ȯ
                if (System.Enum.TryParse(soundData.soundTypeString, out SoundType parsedtype))
                {
                    soundSO.soundType = parsedtype;
                }
                else
                {
                    Debug.LogWarning($"���� '{soundData.id}'�� �������� ���� Ÿ��: {soundData.soundTypeString}");
                }
                soundSO.soundObjectType = soundData.soundObjectType;

                //��ũ���ͺ� ������Ʈ ����
                string assetPath = $"{outputFolder}/Sound_{soundData.id}.asset";
                AssetDatabase.CreateAsset(soundSO, assetPath);

                //�����̸� ����
                soundSO.name = $"sound_{soundData.id}";
                createdSounds.Add(soundSO);

                EditorUtility.SetDirty(soundSO);
            }

            //�����ͺ��̽� ����
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
            Debug.LogError($"JSON ��ȯ ����: {e}");
        }
    }

}

#endif

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class FindObjectsTool : EditorWindow
{
    public const string outputFolder = "Assets/Resources/ScriptableObjects/SpecialSounds";
    private string tag;
    private SpecialObjectsDataSO specialSO;

    [MenuItem("Tools/FindObjectsWithTag")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsTool>("FindObjectsWithTag");
    }
    private void OnGUI()
    {
        GUILayout.Label("Find Objects With Tag", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        tag = EditorGUILayout.TextField("Objects Tag: ", tag);
        EditorGUILayout.Space();

        if (GUILayout.Button("Find Objects With Tag"))
        {
            if (string.IsNullOrEmpty(tag))
            {
                EditorUtility.DisplayDialog("Error", "Please check your code", "OK");
                return;
            }

            tag = tag.ToLower();
            FindObjectsWithTag();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("Check Current Data With Tag"))
        {
            if (string.IsNullOrEmpty(tag))
            {
                EditorUtility.DisplayDialog("Error", "Please check your tag again", "OK");
                return;
            }
            CheckCurrentData();
        }
    }

    private void FindObjectsWithTag()
    {
        GameObject[] soundObjects = FindObjectsOfType<GameObject>();

        switch (tag)             //태그는 소문자만 사용
        {
            case SpecialObjectsDataSO.spikeName:
                SpecialObjectsDataSO.s_spikeTraps.Clear();
                break;

            case SpecialObjectsDataSO.torchName:
                SpecialObjectsDataSO.s_torches.Clear();
                break;
        }

        foreach (GameObject obj in soundObjects)
        {
            if (obj.CompareTag(tag))
            {
                switch (tag)             //태그는 소문자만 사용
                {
                    case SpecialObjectsDataSO.spikeName:
                        SpecialObjectsDataSO.s_spikeTraps.Add(obj);
                    break;

                    case SpecialObjectsDataSO.torchName:
                        SpecialObjectsDataSO.s_torches.Add(obj);
                    break;
                }
            }
        }
        CreateScriptableObject();
    }

    private void CheckCurrentData()
    {
        switch (tag)             //태그는 소문자만 사용
        {
            case SpecialObjectsDataSO.spikeName:
                EditorUtility.DisplayDialog("Success", $"spiketrap의 현재 개수는 {SpecialObjectsDataSO.s_spikeTraps.Count}개이다.", "OK");
                break;

            case SpecialObjectsDataSO.torchName:
                EditorUtility.DisplayDialog("Success", $"횟불의 개수는 {SpecialObjectsDataSO.s_torches.Count}개이다.", "OK");
                Debug.Log($"횟불의 개수는 {SpecialObjectsDataSO.s_torches.Count}개이다.");
                break;

            default:
                EditorUtility.DisplayDialog("Error", $"해당 태그에 맞는 오브젝트가 존재하지 않습니다.", "OK");
                break;
        }
    }

    private void CreateScriptableObject()
    {
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        try
        {
            SpecialObjectsDataSO s_ObjectSO = ScriptableObject.CreateInstance<SpecialObjectsDataSO>();

            string assetPath = $"{outputFolder}/SpecialObjects.asset";
            AssetDatabase.CreateAsset(s_ObjectSO, assetPath);

            EditorUtility.SetDirty(s_ObjectSO);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("Error", $"Failed to Create SO: {e.Message}", "OK");
            Debug.LogError($"SO 생성 오류: {e}");
        }
    }
}
#endif 

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


public class FindObjectsTool : EditorWindow
{
    private string tag;

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
            FindObjectsWithTag();
        }
    }

    private void FindObjectsWithTag()
    {

    }
}
#endif 

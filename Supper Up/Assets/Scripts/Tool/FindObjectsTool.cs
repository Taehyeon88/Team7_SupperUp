using UnityEngine;
using System.Collections.Generic;

public class FindObjectsTool : MonoBehaviour
{
    private static string tag;

    public const string spikeName = "spiketrap";
    public const string torchName = "torch";

    public static List<GameObject> spikeTraps = new List<GameObject>();
    public static List<GameObject> torches = new List<GameObject>();

    public static void FindObjectsWithTag()
    {
        GameObject[] soundObjects = FindObjectsOfType<GameObject>();

        string[] tags = new string[]
        {
            spikeName,
            torchName
        };

        for (int i = 0; i < tags.Length; i++)
        {
            tag = tags[i];

            foreach (GameObject obj in soundObjects)
            {
                if (obj.CompareTag(tag))
                {
                    switch (tag)             //태그는 소문자만 사용
                    {
                        case spikeName: spikeTraps.Add(obj);
                            break;

                        case torchName: torches.Add(obj);
                            break;
                    }
                }
            }
        }
    }

    public static void ResetSetting()
    {
        spikeTraps.Clear();
        torches.Clear();
    }
}
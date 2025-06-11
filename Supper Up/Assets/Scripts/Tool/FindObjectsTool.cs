using UnityEngine;
using System.Collections.Generic;

public class FindObjectsTool : MonoBehaviour
{
    private static string tag;

    public const string spikeName = "spiketrap";
    public const string torchName = "torch";
    public const string cannonName = "cannon";
    public const string d_PlanName = "disappearingplane";  
    public const string fackBridgeName = "fackbridge";
    public const string s_ElevatorName = "selevator";
    public const string w_ElevatorName = "welevator";
    public const string chestName = "chest";
    public const string anvilName = "anvil";
    public const string enemyName = "enemy";

    public static List<GameObject> spikeTraps = new List<GameObject>();
    public static List<GameObject> torches = new List<GameObject>();
    public static List<GameObject> cannons = new List<GameObject>();
    public static List<GameObject> d_Plans = new List<GameObject>();
    public static List<GameObject> fackBridges = new List<GameObject>();
    public static List<GameObject> s_Elevators = new List<GameObject>();
    public static List<GameObject> w_Elevators = new List<GameObject>();
    public static List<GameObject> chests = new List<GameObject>();
    public static List<GameObject> anvils = new List<GameObject>();
    public static List<GameObject> enemys = new List<GameObject>();

    public static void FindObjectsWithTag()
    {
        GameObject[] soundObjects = FindObjectsOfType<GameObject>();

        string[] tags = new string[]
        {
            spikeName,
            torchName,
            cannonName,
            d_PlanName,
            fackBridgeName,
            s_ElevatorName,
            w_ElevatorName,
            chestName,
            anvilName,
            enemyName
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

                        case cannonName: cannons.Add(obj);
                            break;

                        case d_PlanName: d_Plans.Add(obj);
                            break;

                        case fackBridgeName: fackBridges.Add(obj);
                            break;

                        case s_ElevatorName: s_Elevators.Add(obj);
                            break;

                        case w_ElevatorName: w_Elevators.Add(obj);
                            break;

                        case chestName: chests.Add(obj);
                            break;

                        case anvilName: anvils.Add(obj);
                            break;

                        case enemyName: enemys.Add(obj);
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
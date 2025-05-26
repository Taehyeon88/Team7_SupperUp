using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SpecialSoundDatabase")]
public class SpecialObjectsDataSO : ScriptableObject
{
    public const string spikeName = "spiketrap";
    public const string torchName = "torch";

    public static List<GameObject> s_spikeTraps = new List<GameObject>();
    public static List<GameObject> s_torches = new List<GameObject>();

    public List<GameObject> spikeTraps = new List<GameObject>(s_spikeTraps);
    public List<GameObject> torches = new List<GameObject>(s_torches);
}

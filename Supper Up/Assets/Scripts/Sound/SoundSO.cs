using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound" , menuName = "Sound/sounds")]
public class SoundSO : ScriptableObject
{
    public string id;
    public float pitch;
    public float volume;
    public bool loop;
    public bool is3D;
    public float maxDistance;
    public SoundType soundType;
    public string soundObjectType;
    public AudioClip audioClip;

    public override string ToString()
    {
        return $"[{id}] ({soundType}) - 3D���� : {is3D}, �ִ�Ÿ� : {maxDistance}";
    }
}

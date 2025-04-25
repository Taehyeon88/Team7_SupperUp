using System;
using UnityEngine;

[Serializable]
public class SoundData
{
    public string id;
    public float pitch;
    public float volume;
    public bool loop;
    public bool is3D;
    public float maxDistance;
    public string soundTypeString;

    [NonSerialized]
    public SoundType soundType;
    public string soundObjectType;

    public void InitalizeEnums()
    {
        if (Enum.TryParse(soundTypeString, out SoundType parsedType))
        {
            soundType = parsedType;
        }
        else
        {
            Debug.Log($"아이템 '{id}'에 유효하지 않은 아이템 타입 : {soundTypeString}");
            soundType = SoundType.Non;
            //사용하지 않는 타입설정
        }
    }
}

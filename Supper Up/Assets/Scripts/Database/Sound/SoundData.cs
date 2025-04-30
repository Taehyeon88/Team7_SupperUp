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
            Debug.Log($"������ '{id}'�� ��ȿ���� ���� ������ Ÿ�� : {soundTypeString}");
            soundType = SoundType.Non;
            //������� �ʴ� Ÿ�Լ���
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound/Database")]
public class SoundDatabaseSO : ScriptableObject
{
    public List<SoundSO> sounds = new List<SoundSO>();

    //ĳ���� ���� ����
    private Dictionary<string, SoundSO> soundById;

    public void Initialize()
    {
        soundById = new Dictionary<string, SoundSO>();

        foreach (var sound in sounds)
        {
            soundById[sound.id] = sound;
        }
    }

    public SoundSO GetSoundById(string id)
    {
        if (soundById == null)
        {
            Initialize();
        }
        if(soundById.TryGetValue(id, out SoundSO sound))
            return sound;

        return null;
    }

    //Ÿ������ ���� ���͸�
    public SoundSO GetSoundByType(SoundType type)
    {
        return sounds.Find(s => s.soundType == type);
    }
}

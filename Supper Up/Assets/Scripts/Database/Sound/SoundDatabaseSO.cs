using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabase", menuName = "Sound/Database")]
public class SoundDatabaseSO : ScriptableObject
{
    public List<SoundSO> sounds = new List<SoundSO>();

    //캐싱을 위한 사전
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

    //타입으로 사운드 필터링
    public SoundSO GetSoundByType(SoundType type)
    {
        return sounds.Find(s => s.soundType == type);
    }
}

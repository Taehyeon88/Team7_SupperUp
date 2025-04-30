using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private string SOPath = "ScriptableObjects/sounds/SoundDatabase"; //출력 SO 파일 경로값
    void Start()
    {
        SetSounds();
    }

    private void SetSounds()
    {
        SoundDatabaseSO soundDatabaseSO = Resources.Load<SoundDatabaseSO>(SOPath);
        MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();
        GameObject[] soundObjects = monoBehaviours.Select(x => x.gameObject).ToArray();

        if (soundDatabaseSO == null)
        {
            Debug.Log("사운드가 없다");
            return;
        }
        else
        {
            Debug.Log("사운드는 있다");
        }

        foreach (SoundSO sound in soundDatabaseSO.sounds)
        {
            if(string.IsNullOrEmpty(sound.soundObjectType)) continue;

            Type type = System.Type.GetType(sound.soundObjectType);

            foreach (var soundObject in soundObjects)
            {
                if (soundObject.GetComponent(type) != null)
                {
                    AudioSource audio = soundObject.AddComponent<AudioSource>();
                    audio.pitch = sound.pitch;
                    audio.volume = sound.volume;
                    audio.loop = sound.loop;
                    audio.spatialBlend = sound.is3D ? 1 : 0;
                    audio.maxDistance = sound.maxDistance;
                    audio.clip = sound.audioClip ? sound.audioClip : null;
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private string SOPath = "ScriptableObjects/sounds/SoundDatabase"; //��� SO ���� ��ΰ�
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
            Debug.Log("���尡 ����");
            return;
        }
        else
        {
            Debug.Log("����� �ִ�");
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

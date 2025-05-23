using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f,1f)] public float volume;

    [Range(0.1f,3f)] public float pitch = 1.0f;
    public bool loop;
    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<Sound> sounds = new List<Sound>();
    public AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.mixerGroup;
        }

        //PlaySound("Walk");
    }

    public void PlaySound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);

        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
    }

    public void PauseSound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);

        if (soundToPlay != null)
        {
            soundToPlay.source.Pause();
        }
    }

    public bool CheckSoundPlay(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);

        if (soundToPlay != null)
        {
            return soundToPlay.source.isPlaying;
        }
        return false;
    }

    public void IncreasePitch(string name, float value)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);

        if (soundToPlay != null)
        {
            soundToPlay.source.pitch = soundToPlay.pitch * value;
        }
    }

    public void FadeSound(string name, float value, bool isUseStop = false)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);
        float fadeDuration = 0.2f;

        if (soundToPlay != null)
        {
            if (value == 1)
            {
                soundToPlay.source.Play();
                soundToPlay.source.DOFade(value, fadeDuration);
            }
            else
            {
                if(!isUseStop) soundToPlay.source.DOFade(value, fadeDuration).OnComplete(() => soundToPlay.source.Pause());
                else soundToPlay.source.DOFade(value, fadeDuration).OnComplete(() => soundToPlay.source.Stop());
            }
        }
    }
}

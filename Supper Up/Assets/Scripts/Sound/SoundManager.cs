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
    public bool is3D;
    public bool isSpecial = false;
    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<Sound> sounds = new List<Sound>();
    public AudioMixer audioMixer;

    private List<AudioSource> soundSources = new List<AudioSource>();
    private List<AudioSource> playingSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        soundSources.Clear();
        FindObjectsTool.ResetSetting();
        FindObjectsTool.FindObjectsWithTag();

        List<GameObject> spikeTraps2 = new List<GameObject>(FindObjectsTool.spikeTraps);
        List<GameObject> torches2 = new List<GameObject>(FindObjectsTool.torches);

        Debug.Log(FindObjectsTool.spikeTraps.Count);
        Debug.Log(FindObjectsTool.torches.Count);

        foreach (Sound sound in sounds)
        {
            if (!sound.isSpecial)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.outputAudioMixerGroup = sound.mixerGroup;

                soundSources.Add(sound.source);
            }
            else
            {
                string name;
                string temp;
                if (sound.name.Contains("_"))
                {
                    temp = sound.name.Substring(sound.name.IndexOf("_"));
                    name = sound.name.Replace(temp, "").ToLower();
                }
                else
                {
                    name = sound.name.ToLower();
                }
                //Debug.Log("특별한 사운드의 이름은 :" + name);

                switch (name)
                {
                    case FindObjectsTool.spikeName:
                        foreach (var obj in FindObjectsTool.spikeTraps)
                        {
                            //Debug.Log("된다");
                            sound.source = obj.AddComponent<AudioSource>();
                            sound.source.clip = sound.clip;
                            sound.source.volume = sound.volume;
                            sound.source.pitch = sound.pitch;
                            sound.source.loop = sound.loop;
                            sound.source.maxDistance = 10f;
                            sound.source.rolloffMode = AudioRolloffMode.Linear;
                            if (sound.is3D) sound.source.spatialBlend = 1f;
                            sound.source.outputAudioMixerGroup = sound.mixerGroup;

                            soundSources.Add(sound.source);
                        }
                        break;

                    case FindObjectsTool.torchName:
                        foreach (var obj in FindObjectsTool.torches)
                        {
                            sound.source = obj.AddComponent<AudioSource>();
                            sound.source.clip = sound.clip;
                            sound.source.volume = sound.volume;
                            sound.source.pitch = sound.pitch;
                            sound.source.loop = sound.loop;
                            sound.source.maxDistance = 10f;
                            sound.source.rolloffMode = AudioRolloffMode.Linear;
                            if (sound.is3D) sound.source.spatialBlend = 1f;
                            sound.source.outputAudioMixerGroup = sound.mixerGroup;

                            sound.source.Play();

                            soundSources.Add(sound.source);
                        }
                        break;
                }
            }
        }

        Debug.Log($"총 사운드의 개수는 : {soundSources.Count}");
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
                if (!isUseStop) soundToPlay.source.DOFade(value, fadeDuration).OnComplete(() => soundToPlay.source.Pause());
                else soundToPlay.source.DOFade(value, fadeDuration).OnComplete(() => soundToPlay.source.Stop());
            }
        }
    }

    public void FadeSound_S(AudioSource audioSource, float value, bool isUseStop = false)
    {
        float fadeDuration = 0.2f;

        if (audioSource != null)
        {
            if (value == 1)
            {
                audioSource.Play();
                audioSource.DOFade(value, fadeDuration);
            }
            else
            {
                if (!isUseStop) audioSource.DOFade(value, fadeDuration).OnComplete(() => audioSource.Pause());
                else audioSource.DOFade(value, fadeDuration).OnComplete(() => audioSource.Stop());
            }
        }
    }

    public string FindAudioWithClip(AudioClip clip)
    {
        Sound sound = sounds.Find(sound => sound.clip == clip);

        if (sound != null)
        {
            string name = sound.name;
            name = name.Substring(name.IndexOf("_") + 1);

            return name;
        }
        return " ";
    }

    public void PauseAllSounds()
    {
        playingSources.Clear();
        foreach (var soundSource in soundSources)
        {
            if (soundSource.isPlaying)
            {
                playingSources.Add(soundSource);
                soundSource.Pause();
            }
        }
    }

    public void RePlayAllSounds()
    {
        foreach (var soundSource in playingSources)
        {
            FadeSound_S(soundSource, 1f);
        }
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Sound[] sounds;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    // Start is called before the first frame update
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}

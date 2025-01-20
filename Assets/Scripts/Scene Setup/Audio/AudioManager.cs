using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance; // ensures loading into another scene doesn't spawn another

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) // if no audiomanager exists
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // prevent redundant audiomanagers
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            // Set Unity AudioSource settings to Sound class parameters
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play("Sample Music");
    }

    public void Play(string playName)
    {
        // in sounds[], find sound where its name is playName
        Sound s = Array.Find(sounds, sound => sound.name == playName);
        if (s == null)
        {
            Debug.LogWarning("Hey idiot, sound ~~" + name + "~~ was not found.");
            return;
        }
        s.source.Play();
    }

    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.Stop();
    }
}

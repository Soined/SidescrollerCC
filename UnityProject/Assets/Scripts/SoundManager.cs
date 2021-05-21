using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Main;

    private void Awake()
    {
        if(Main == null)
        {
            Main = this;
        } else if(Main != this)
        {
            Destroy(this);
        }
    }

    public SoundResources resource;

    [SerializeField]
    [Range(0, 1)]
    private float volume;

    private List<AudioSource> allSFXsources = new List<AudioSource>();


    private void Update()
    {
        List<AudioSource> finishedPlaying = new List<AudioSource>();
        foreach(AudioSource source in allSFXsources)
        {
            if(!source.isPlaying)
            {
                finishedPlaying.Add(source);
            }
        }
        foreach(AudioSource source in finishedPlaying)
        {
            allSFXsources.Remove(source);
            Destroy(source);
        }
    }

    public void PlayNewSound(AudioClip clip, bool loop = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
        source.Play();
        allSFXsources.Add(source);
    }
    public void PlayNewSound(SoundType type, bool loop = false)
    {
        AudioClip clip = null;
        switch(type)
        {
            case SoundType.playerJump:
                clip = resource.playerJump;
                break;
            case SoundType.playerShoot:
                clip = resource.playerShoot;
                break;
        }
        PlayNewSound(clip, loop);
    }
}


public enum SoundType
{
    playerJump,
    playerShoot
}
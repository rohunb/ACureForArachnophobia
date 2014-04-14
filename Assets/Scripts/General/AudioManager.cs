using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public enum Sound { MP5, Shotgun, LightningGun, Flamethrower, HealingBeam, SoldierHurt, SoldierDeath, SpiderAttack, SpiderDeath, BackgroundTrack }

    public AudioClip clip_MP5;
    public AudioClip clip_Shotgun;
    public AudioClip clip_LightningGun;
    public AudioClip clip_Flamethrower;
    public AudioClip clip_HealingBeam;
    public AudioClip clip_SoldierHurt;
    public AudioClip clip_SoldierDeath;
    public AudioClip clip_SpiderAttack;
    public AudioClip clip_SpiderDeath;
    public AudioClip clip_BackgroundTrack;

    AudioSource[] sources;
    public int numSources = 16;

    private static AudioManager instance = null;

    public static AudioManager Instance
    {
        get { return instance; }
    }


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(name + ": error: already initialized", this);
        }
        instance = this;
    }

    void Start()
    {
        sources = new AudioSource[numSources];
        for (int i = 0; i < numSources; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(Sound sound, bool loop)
    {
        AudioClip audioClip = null;
        switch (sound)
        {
            case Sound.MP5:
                audioClip = clip_MP5;
                break;
            case Sound.Shotgun:
                audioClip = clip_Shotgun;
                break;
            case Sound.LightningGun:
                audioClip = clip_LightningGun;
                break;
            case Sound.Flamethrower:
                audioClip = clip_Flamethrower;
                break;
            case Sound.HealingBeam:
                audioClip = clip_HealingBeam;
                break;
            case Sound.SoldierHurt:
                audioClip = clip_SoldierHurt;
                break;
            case Sound.SoldierDeath:
                audioClip = clip_SoldierDeath;
                break;
            case Sound.SpiderAttack:
                audioClip = clip_SpiderAttack;
                break;
            case Sound.SpiderDeath:
                audioClip = clip_SpiderDeath;
                break;
            case Sound.BackgroundTrack:
                audioClip = clip_BackgroundTrack;
                break;
            default:
                break;
        }
        if (audioClip)
        {
            for (int i = 0; i < numSources; i++)
            {
                AudioSource source = sources[i];
                if (!source.isPlaying)
                {
                    source.clip = audioClip;
                    source.loop = loop;
                    source.Play();
                    return;
                }
            }
        }

    }
    public void PlaySound(Sound sound, float vol, bool loop)
    {
        AudioClip audioClip = null;
        switch (sound)
        {
            case Sound.MP5:
                audioClip = clip_MP5;
                break;
            case Sound.Shotgun:
                audioClip = clip_Shotgun;
                break;
            case Sound.LightningGun:
                audioClip = clip_LightningGun;
                break;
            case Sound.Flamethrower:
                audioClip = clip_Flamethrower;
                break;
            case Sound.HealingBeam:
                audioClip = clip_HealingBeam;
                break;
            case Sound.SoldierHurt:
                audioClip = clip_SoldierHurt;
                break;
            case Sound.SoldierDeath:
                audioClip = clip_SoldierDeath;
                break;
            case Sound.SpiderAttack:
                audioClip = clip_SpiderAttack;
                break;
            case Sound.SpiderDeath:
                audioClip = clip_SpiderDeath;
                break;
            case Sound.BackgroundTrack:
                audioClip = clip_BackgroundTrack;
                break;
            default:
                break;
        }
        if (audioClip)
        {
            for (int i = 0; i < numSources; i++)
            {
                AudioSource source = sources[i];
                if (!source.isPlaying)
                {
                    source.clip = audioClip;
                    source.loop = loop;
                    source.volume = vol;
                    source.Play();
                    return;
                }
            }
        }

    }
    public void StopSound(Sound sound)
    {
        AudioClip audioClip = null;
        switch (sound)
        {
            case Sound.MP5:
                audioClip = clip_MP5;
                break;
            case Sound.Shotgun:
                audioClip = clip_Shotgun;
                break;
            case Sound.LightningGun:
                audioClip = clip_LightningGun;
                break;
            case Sound.Flamethrower:
                audioClip = clip_Flamethrower;
                break;
            case Sound.HealingBeam:
                audioClip = clip_HealingBeam;
                break;
            case Sound.SoldierHurt:
                audioClip = clip_SoldierHurt;
                break;
            case Sound.SoldierDeath:
                audioClip = clip_SoldierDeath;
                break;
            case Sound.SpiderAttack:
                audioClip = clip_SpiderAttack;
                break;
            case Sound.SpiderDeath:
                audioClip = clip_SpiderDeath;
                break;
            case Sound.BackgroundTrack:
                audioClip = clip_BackgroundTrack;
                break;
            default:
                break;
        }
        if (audioClip)
        {
            foreach (AudioSource source in sources)
            {
                if (source.clip == audioClip && source.isPlaying)
                {
                    source.Stop();
                }
            }
        }
    }

}


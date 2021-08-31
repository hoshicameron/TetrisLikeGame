using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip[] backgroundMusicAudioClipArray;

    [Header("Vocals")] [SerializeField] private AudioClip[] vocalAudioClipArray;
    [SerializeField] private AudioClip vocalLevelUpAudioClip;
    [SerializeField] private AudioClip vocalGameOverAudioClip;



    [Header("SFX")]
    [SerializeField] private AudioClip clearRowAudioClip;
    [SerializeField] private AudioClip moveAudioClip;
    [SerializeField] private AudioClip gameOverAudioClip;
    [SerializeField] private AudioClip dropAudioClip;
    [SerializeField] private AudioClip errorAudioClip;
    [SerializeField] private AudioClip transformerAudioClip;
    [SerializeField] private AudioClip levelUpSfxAudioClip;

    [Header("MixerGroups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup vocalMixerGroup;


    private AudioSource musicAudioSource;
    private AudioSource sfxAudioSource;
    private AudioSource vocalAudioSource;

    [Range(0.0f,1.0f)][SerializeField] private float musicVolume = 1.0f;

    [Range(0.0f,1.0f)][SerializeField] private float sfxVolume = 1.0f;

    [Range(0.0f,1.0f)][SerializeField] private float vocalVolume = 1.0f;

    private bool musicEnabled =true;
    private bool sfxEnabled = true;
    private bool vocalEnabled = true;

    private void Awake()
    {
        Instance = this;
        musicAudioSource=gameObject.AddComponent<AudioSource>()as AudioSource;
        sfxAudioSource=gameObject.AddComponent<AudioSource>()as AudioSource;
        vocalAudioSource=gameObject.AddComponent<AudioSource>()as AudioSource;

        musicAudioSource.outputAudioMixerGroup = musicMixerGroup;
        sfxAudioSource.outputAudioMixerGroup = sfxMixerGroup;
        vocalAudioSource.outputAudioMixerGroup = vocalMixerGroup;

        sfxAudioSource.loop = false;
    }

    public void PlayMusic()
    {
        if(musicAudioSource.clip==null)
            musicAudioSource.clip = backgroundMusicAudioClipArray[Random.Range(0, backgroundMusicAudioClipArray.Length)];
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        if(musicAudioSource.isPlaying)
            musicAudioSource.Stop();
    }

    private void UpdateMusic()
    {
        if (musicAudioSource.isPlaying != musicEnabled)
        {
            if (musicEnabled)
            {
                PlayMusic();
            } else
            {
                StopMusic();
            }
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        UpdateMusic();
    }

    public void ToggleSFX()
    {
        sfxEnabled = !sfxEnabled;
        UpdateSFX();
    }

    private void UpdateSFX()
    {
        if (sfxEnabled ||vocalEnabled)
        {
            sfxMixerGroup.audioMixer.SetFloat("sfxVolume", 0f);
            vocalMixerGroup.audioMixer.SetFloat("vocalVolume", 0f);
        } else
        {
            sfxMixerGroup.audioMixer.SetFloat("sfxVolume", -80f);
            vocalMixerGroup.audioMixer.SetFloat("vocalVolume", -80f);
        }


    }

    public void PlayMoveAudioClip()
    {
        sfxAudioSource.PlayOneShot(moveAudioClip);
    }

    public void PlayClearRowAudioClip()
    {
        sfxAudioSource.PlayOneShot(clearRowAudioClip);

    }

    public void PlayDropAudioClip()
    {
        sfxAudioSource.PlayOneShot(dropAudioClip);

    }

    public void PlayGameOverAudioClip()
    {
        sfxAudioSource.PlayOneShot(gameOverAudioClip);

    }

    public void PlayErrorAudioClip()
    {
        sfxAudioSource.PlayOneShot(errorAudioClip);

    }

    public void PlayVocalClip()
    {
        vocalAudioSource.PlayOneShot(vocalAudioClipArray[Random.Range(0,vocalAudioClipArray.Length)]);
    }

    public void PlayVocalGameOverClip()
    {
        vocalAudioSource.PlayOneShot(vocalGameOverAudioClip);
    }


}

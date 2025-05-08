using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private List<AudioSource> musicsSources = new List<AudioSource>();
    private float musicsVolume = 1f;
    private int musicsPoolSize = 2;
    private List<AudioSource> soundsSources = new List<AudioSource>();
    private int soundsPoolSize = 10;
    private float soundsVolume = 1f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        for (int i = 0; i < musicsPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f; // 2D par défaut
            source.loop = true;
            musicsSources.Add(source);
        }
        for (int i = 0; i < soundsPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f; // 2D par défaut
            source.loop = false;
            soundsSources.Add(source);
        }
    }
    
    #region Musique
    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        AudioSource source = GetAvailableMusicSource();
        if (source == null)
        {
            return;
        }
        source.clip = clip;
        source.volume = musicsVolume * volume;
        source.spatialBlend = 0f; // 2D
        source.Play();
    }
    public void StopMusic()
    {
        foreach (AudioSource source in musicsSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
    public void PauseMusic()
    {
        foreach (AudioSource source in musicsSources)
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
        }
    }
    public void ResumeMusic()
    {
        foreach (AudioSource source in musicsSources)
        {
            if (source.isPlaying)
            {
                source.UnPause();
            }
        }
    }
    public void MuteMusic(bool mute)
    {
        foreach (AudioSource source in musicsSources)
        {
            source.mute = mute;
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicsVolume = Mathf.Clamp01(volume);
        foreach (AudioSource source in musicsSources)
        {
            if (source.isPlaying)
            {
                source.volume = musicsVolume;
            }
        }
    }
    #endregion
    
    #region Sounds
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        AudioSource source = GetAvailableSoundSource();
        if (source == null)
        {
            return;
        }
        source.clip = clip;
        source.volume = soundsVolume * volume;
        source.spatialBlend = 0f; // 2D
        source.Play();
    }
    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource source = GetAvailableSoundSource();
        if (source == null)
        {
            return;
        }
        source.transform.position = position;
        source.clip = clip;
        source.volume = soundsVolume * volume;
        source.spatialBlend = 1f; // 3D
        source.Play();
    }
    public void SetSoundsVolume(float volume)
    {
        soundsVolume = Mathf.Clamp01(volume);
        foreach (AudioSource source in soundsSources)
        {
            if (source.isPlaying)
            {
                source.volume = soundsVolume;
            }
        }
    }
    public void MuteSounds(bool mute)
    {
        foreach (AudioSource source in soundsSources)
        {
            source.mute = mute;
        }
    }
    public void StopAllSounds()
    {
        foreach (AudioSource source in soundsSources)
        {
            source.Stop();
        }
    }
    #endregion
    
    #region Utils
    private AudioSource GetAvailableMusicSource()
    {
        foreach (var source in musicsSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
    private AudioSource GetAvailableSoundSource()
    {
        foreach (var source in soundsSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
    private AudioSource GetPlayingMusicSource()
    {
        foreach (AudioSource source in musicsSources)
        {
            if (source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
    private AudioSource GetPlayingSoundSource()
    {
        foreach (AudioSource source in soundsSources)
        {
            if (source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
    #endregion
    
    #region Fades & Transitions
    public void FadeInMusic(AudioClip clip, float duration = 2f, float targetVolume = 1f)
    {
        StopCoroutine(nameof(FadeInMusicRoutine));
        StartCoroutine(FadeInMusicRoutine(clip, duration, targetVolume));
    }
    private IEnumerator FadeInMusicRoutine(AudioClip clip, float duration, float targetVolume)
    {
        AudioSource source = GetAvailableMusicSource();
        if (!source)
        {
            yield break;
        }
        source.clip = clip;
        source.volume = 0f;
        source.Play();
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }
        source.volume = targetVolume;
        musicsVolume = targetVolume;
    }
    public void FadeOutMusic(float duration = 2f)
    {
        StopCoroutine(nameof(FadeOutMusicRoutine));
        StartCoroutine(FadeOutMusicRoutine(duration));
    }
    private IEnumerator FadeOutMusicRoutine(float duration)
    {
        AudioSource source = GetAvailableMusicSource();
        if (!source)
        {
            yield break;
        }
        float startVolume = source.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }
        source.Stop();
        source.volume = musicsVolume;
    }
    public void FadeInOutMusic(AudioClip clip, float duration = 2f, float targetVolume = 1f)
    {
        StopCoroutine(nameof(FadeInOutMusicRoutine));
        StartCoroutine(FadeInOutMusicRoutine(clip, duration, targetVolume));
    }
    private IEnumerator FadeInOutMusicRoutine(AudioClip clip, float duration, float targetVolume)
    {
        StopCoroutine(nameof(FadeOutMusicRoutine));
        yield return StartCoroutine(FadeOutMusicRoutine(duration));
        StopCoroutine(nameof(FadeInMusicRoutine));
        yield return StartCoroutine(FadeInMusicRoutine(clip, duration, targetVolume));
    }
    public void FadeCrossMusic(AudioClip clip, float duration = 2f, float targetVolume = 1f)
    {
        FadeOutMusic(duration);
        FadeInMusic(clip, duration, targetVolume);
    }
    #endregion
}

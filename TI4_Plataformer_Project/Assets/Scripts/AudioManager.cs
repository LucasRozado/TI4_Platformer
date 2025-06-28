using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static private AudioManager instance;

    [SerializeField] private AudioClip music;
    [SerializeField] private bool playOnlyOnce;

    [SerializeField] private AudioSource musicManager;
    [SerializeField] private AudioSource sfxManager;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayDefault();
        }
        else
        {
            instance.music = music;
            instance.playOnlyOnce = playOnlyOnce;
            PlayDefault();
            Destroy(gameObject);
        }
    }


    static public _SFX SFX => instance.sFX;
    [SerializeField] public _SFX sFX;
    [System.Serializable]
    public class _SFX
    {
        [SerializeField] public AudioClip playerStep;
    }


    static public void TocarSFX(AudioClip sfx)
    => instance._TocarSFX(sfx);
    private void _TocarSFX(AudioClip sfx)
    => sfxManager.PlayOneShot(sfx);


    static public void PlayDefault()
    => instance._PlayDefault();
    private void _PlayDefault()
    {
        if (music == null) return;

        if (playOnlyOnce) PlayMusicOnce(music);
        else PlayMusicLoop(music);
    }

    static public void PlayMusicOnce(AudioClip music)
    => instance._PlayMusicOnce(music);
    private void _PlayMusicOnce(AudioClip music)
    {
        musicManager.loop = false;
        musicManager.clip = music;
        musicManager.Play();
    }

    static public void PlayMusicLoop(AudioClip musica)
    => instance._PlayMusicLoop(musica);
    private void _PlayMusicLoop(AudioClip musica)
    {
        if (musicManager.clip == musica && musicManager.isPlaying) return;

        musicManager.loop = true;
        musicManager.clip = musica;
        musicManager.Play();
    }

    static public void StopMusic()
    => instance._StopMusic();
    private void _StopMusic()
    { musicManager.Stop(); }

    static private float ConvertVolume(float porcentagem)
    {
        float valorDecimal = porcentagem / 100;
        // o humano escuta em uma escala logarítmica
        // a conta abaixo converte o valor linear para a escala humana
        return valorDecimal == 0 ? -80f : Mathf.Log10(valorDecimal) * 20;
    }

    static private int volumeMaster = 100;
    static public int GetVolumeMaster() => volumeMaster;
    static public void SetVolumeMaster(float percentage)
    => instance._SetVolumeMaster(percentage);
    private void _SetVolumeMaster(float percentage)
    {
        float volume = ConvertVolume(percentage);
        audioMixer.SetFloat("VolumeMaster", volume);
    }

    static private int volumeMusic = 100;
    static public int GetVolumeMusic() => volumeMusic;
    static public void SetVolumeMusic(float percentage)
    => instance._SetVolumeMusic(percentage);
    private void _SetVolumeMusic(float percentage)
    {
        float volume = ConvertVolume(percentage);
        audioMixer.SetFloat("VolumeMusic", volume);
    }

    static private int volumeSFX = 100;
    static public int GetVolumeSFX() => volumeSFX;
    static public void SetVolumeSFX(float porcentagem)
    => instance._SetVolumeSFX(porcentagem);
    private void _SetVolumeSFX(float porcentagem)
    {
        float volume = ConvertVolume(porcentagem);
        audioMixer.SetFloat("VolumeSFX", volume);
    }
}

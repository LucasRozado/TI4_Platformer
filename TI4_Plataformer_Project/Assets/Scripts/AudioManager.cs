using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static private AudioManager instance;

    [SerializeField] private AudioClip musica;
    [SerializeField] private bool ApenasUmaVez;

    [SerializeField] private AudioSource gerenciadorMusica;
    [SerializeField] private AudioSource gerenciadorVozes;
    [SerializeField] private AudioSource gerenciadorSFX;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            TocarPredefinida();
        }
        else
        {
            instance.musica = musica;
            instance.ApenasUmaVez = ApenasUmaVez;
            TocarPredefinida();
            Destroy(gameObject);
        }
    }


    static public _SFX SFX => instance.sFX;
    [SerializeField] public _SFX sFX;
    [System.Serializable]
    public class _SFX
    {
        [SerializeField] public AudioClip buttonClick;
    }


    static public void TocarSFX(AudioClip sfx)
    => instance._TocarSFX(sfx);
    private void _TocarSFX(AudioClip sfx)
    => gerenciadorSFX.PlayOneShot(sfx);


    static public void TocarPredefinida()
    => instance._TocarPredefinida();
    private void _TocarPredefinida()
    {
        if (musica == null) return;

        if (ApenasUmaVez) TocarMusicaUmaVez(musica);
        else TocarMusicaEmLoop(musica);
    }

    static public void TocarMusicaUmaVez(AudioClip musica)
    => instance._TocarMusicaUmaVez(musica);
    private void _TocarMusicaUmaVez(AudioClip musica)
    {
        gerenciadorMusica.loop = false;
        gerenciadorMusica.clip = musica;
        gerenciadorMusica.Play();
    }

    static public void TocarMusicaEmLoop(AudioClip musica)
    => instance._TocarMusicaDeFundo(musica);
    private void _TocarMusicaDeFundo(AudioClip musica)
    {
        if (gerenciadorMusica.clip == musica && gerenciadorMusica.isPlaying) return;

        gerenciadorMusica.loop = true;
        gerenciadorMusica.clip = musica;
        gerenciadorMusica.Play();
    }

    static public void PararMusica()
    => instance._PararMusica();
    private void _PararMusica()
    { gerenciadorMusica.Stop(); }

    static private float ConverteVolume(float porcentagem)
    {
        float valorDecimal = porcentagem / 100;
        // o humano escuta em uma escala logarítmica
        // a conta abaixo converte o valor linear para a escala humana
        return valorDecimal == 0 ? -80f : Mathf.Log10(valorDecimal) * 20;
    }

    static private int volumeGeral = 100;
    static public int GetVolumeGeral() => volumeGeral;
    static public void SetVolumeGeral(float porcentagem)
    => instance._SetVolumeGeral(porcentagem);
    private void _SetVolumeGeral(float porcentagem)
    {
        float volume = ConverteVolume(porcentagem);
        audioMixer.SetFloat("VolumeGeral", volume);
    }

    static private int volumeMusica = 100;
    static public int GetVolumeMusica() => volumeMusica;
    static public void SetVolumeMusica(float porcentagem)
    => instance._SetVolumeMusica(porcentagem);
    private void _SetVolumeMusica(float porcentagem)
    {
        float volume = ConverteVolume(porcentagem);
        audioMixer.SetFloat("VolumeMusica", volume);
    }

    static private int volumeVozes = 100;
    static public int GetVolumeVozes() => volumeVozes;
    static public void SetVolumeVozes(float porcentagem)
    => instance._SetVolumeVozes(porcentagem);
    private void _SetVolumeVozes(float porcentagem)
    {
        float volume = ConverteVolume(porcentagem);
        audioMixer.SetFloat("VolumeVozes", volume);
    }

    static private int volumeSFX = 100;
    static public int GetVolumeSFX() => volumeSFX;
    static public void SetVolumeSFX(float porcentagem)
    => instance._SetVolumeSFX(porcentagem);
    private void _SetVolumeSFX(float porcentagem)
    {
        float volume = ConverteVolume(porcentagem);
        audioMixer.SetFloat("VolumeSFX", volume);
    }
}

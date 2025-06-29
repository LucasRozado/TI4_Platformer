using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    public static GlobalSound instance;
    [SerializeField] AudioSource source;
    [SerializeField] float volume = 1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
    public void PlayClip(AudioClip clip, float volume)
    {
        source.PlayOneShot(clip, volume);
    }
    public void PlayClip(AudioClip clip, float volume, bool loop)
    {
        source.loop = loop;
        source.volume = volume;
        source.clip = clip;
        if (!source.isPlaying)
        {
            source.Play();
        }
    }
    public void StopClip()
    {
        source.Stop();
        source.loop = false;
        source.clip = null;
    }
    public void StopClip(AudioClip clip)
    {
        if (source.clip == clip)
        {
            source.Stop();
            source.loop = false;
            source.clip = null;
        }
    }

}

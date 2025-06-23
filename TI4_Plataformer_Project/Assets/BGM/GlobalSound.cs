using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    public static GlobalSound instance;
    [SerializeField] AudioSource source;
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
}

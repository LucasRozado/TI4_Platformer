using UnityEngine;

public class HUBAudioTrigger : MonoBehaviour
{
    [SerializeField] AudioClip levelSong;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.PlayMusicLoop(levelSong);
            Destroy(gameObject);
        }
    }
}

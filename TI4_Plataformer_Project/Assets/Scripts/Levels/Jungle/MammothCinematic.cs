using UnityEngine;

public class MammothCinematic : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;
    [SerializeField] GameObject mammoth;
    [SerializeField] Animator animator;
    Vector3 playerPos;
    bool isActive;
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            isActive = true;
            animator.SetTrigger("Cinematic");
            mammoth.SetActive(false);
            StopPlayer();
            //BrainStatic.instance.cinemachine.Target.TrackingTarget = cameraTarget;
        }
    }

    public void EndCinematic()
    {
        mammoth.SetActive(true);
        isActive = false;
        Destroy(gameObject);
    }
    public void StopPlayer()
    {
        playerPos = Player.instance.transform.position;
    }

    public void Update()
    {
        if (isActive)
            Player.instance.transform.position = playerPos;
    }
}

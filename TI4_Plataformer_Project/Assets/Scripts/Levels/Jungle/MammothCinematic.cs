using UnityEngine;
using UnityEngine.Playables;

public class MammothCinematic : Progress
{
    [SerializeField] GameObject mammoth;
    [SerializeField] PlayableDirector director;

    bool isActive;

    private void OnEnable()
    {
        if (LevelProgress.instance.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActive && other.CompareTag("Player"))
        {
            isActive = true;
            director.Play();
            //LevelProgress.instance.Activate(intReference);
        }
    }

    public void ControlCinematic(bool setActive)
    {
        mammoth.SetActive(setActive);
        Player.instance.gameObject.SetActive(setActive);
        BrainStatic.instance.gameObject.SetActive(setActive);

        if (setActive == true)
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class TutorialTrigger : Progress
{
    [SerializeField] string tutorialText;
    [SerializeField] float duration;
    [SerializeField] bool lastTutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.ShowText(tutorialText, duration);
            gameObject.SetActive(false);
            if (lastTutorial)
            {
                levelProgress.Activate(intReference);
            }
        }
    }

    private void OnEnable()
    {
        if (levelProgress.GetProgress(intReference))
        {
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected string tutorialText = "Press E to Interact";
    [SerializeField] protected float tutorialDuration = 5f;
    [SerializeField] protected float timeToTutorial = 2f;
    protected float timer;
    public abstract void InteractWith(Player player);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        timer = timeToTutorial;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            timer = timeToTutorial;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                UIManager.instance.ShowText(tutorialText, tutorialDuration);
                timer = tutorialDuration;
            }
        }        
    }
}
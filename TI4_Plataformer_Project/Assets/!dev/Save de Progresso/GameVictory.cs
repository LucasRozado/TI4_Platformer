using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameVictory : MonoBehaviour
{
    [SerializeField] Image whiteScreen;
    [SerializeField] float duration;
    bool isSoundDone;
    [SerializeField] AudioClip finalRoar;
    [SerializeField] GameObject jungleCompletionVFX;
    [SerializeField] GameObject canionCompletionVFX;
    [SerializeField] GameObject completionVFX;

    private void Start()
    {
        if (GameManager.endGame.GetLevel(CollectableType.Jungle))
        {
            jungleCompletionVFX.SetActive(true);
        }
        if (GameManager.endGame.GetLevel(CollectableType.Canion))
        {
            canionCompletionVFX.SetActive(true);
        }
        if (GameManager.endGame.GetLevel(CollectableType.Jungle) && GameManager.endGame.GetLevel(CollectableType.Canion))
        {
            completionVFX.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.endGame.GetLevel(CollectableType.Jungle) && GameManager.endGame.GetLevel(CollectableType.Canion))
            {
                StartCoroutine(GameEnd());
            }
        }        
    }

    public IEnumerator GameEnd()
    {
        whiteScreen.gameObject.SetActive(true);
        float t = 0;
        while (t <= duration)
        {
            whiteScreen.color = Color.Lerp(whiteScreen.color, Color.white, t/duration);
            t += Time.deltaTime;
            if (!isSoundDone && t >= duration/2)
            {
                isSoundDone = true;
                GlobalSound.instance.PlayClip(finalRoar, 1);
            }
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(0);
        yield return null;
    }
}

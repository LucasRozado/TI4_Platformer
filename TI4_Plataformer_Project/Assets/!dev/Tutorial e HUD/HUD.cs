using TMPro;
using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] collectablesText;
    [SerializeField] TextMeshProUGUI screenText;
    [SerializeField] float collectableDuration = 2f;

    private void Start()
    {
        foreach(TextMeshProUGUI collectable in collectablesText)
        {
            collectable.gameObject.SetActive(false);
        }
        screenText.gameObject.SetActive(false);
        DisplayAllCollectables();
    }

    public void UpdateCollectables(CollectableType type)
    {
        foreach (TextMeshProUGUI collectable in collectablesText)
        {
            collectable.gameObject.SetActive(false);
        }
        StopCoroutine(DisplayCollectableCR(type, collectableDuration));
        StopCoroutine(DisplayAllCollectablesCR(collectableDuration));
        StartCoroutine(DisplayCollectableCR(type, collectableDuration));
    }
    public IEnumerator DisplayCollectableCR(CollectableType type, float duration)
    {
        collectablesText[(int)type].gameObject.SetActive(true);
        collectablesText[(int)type].text = GameManager.collectableManager.GetScore(type).ToString() + "/100";
        yield return new WaitForSeconds(duration);
        collectablesText[(int)type].gameObject.SetActive(false);
    }

    public void DisplayAllCollectables()
    {
        StopAllCoroutines();
        StartCoroutine(DisplayAllCollectablesCR(collectableDuration));
    }

    public IEnumerator DisplayAllCollectablesCR(float duration)
    {
        for (int i = 0; i < collectablesText.Length; i++)
        {
            collectablesText[i].gameObject.SetActive(true);
            collectablesText[i].text = GameManager.collectableManager.GetScore((CollectableType)i).ToString() + "/100";
        }
        
        yield return new WaitForSeconds(duration);
        for (int i = 0; i < collectablesText.Length; i++)
        {
            collectablesText[i].gameObject.SetActive(false);
        }
    }

    public void DisplayText(string text, float duration)
    {
        StopCoroutine(DisplayTextCR(text, duration));
        StartCoroutine(DisplayTextCR(text, duration));
    }

    public IEnumerator DisplayTextCR(string text, float duration)
    {
        screenText.gameObject.SetActive(true);
        screenText.text = text;
        yield return new WaitForSeconds(duration);
        screenText.gameObject.SetActive(false);
    }
}

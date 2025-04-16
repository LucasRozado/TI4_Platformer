using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class FastTravelScreen : MonoBehaviour
{
    [SerializeField] Button travel;
    [SerializeField] Image travelPreview;
    [SerializeField] CPButton[] fastTravelButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void CloseFastTravel()
    {
        travel.gameObject.SetActive(false);
        travelPreview.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void PrepareFastTravel(CPButton button)
    {
        travel.gameObject.SetActive(true);
        travelPreview.gameObject.SetActive(true);
        travelPreview.sprite = button.checkpoint.sceneImage;
        travel.onClick.RemoveAllListeners();
        travel.onClick.AddListener(button.Travel);
    }

    public void OnEnable()
    {
        travelPreview.gameObject.SetActive(false);
        travel.gameObject.SetActive(false);
        foreach (CPButton button in fastTravelButtons)
        {
            button.gameObject.SetActive(GameManager.Instance.checkpointManager.VerifyCheckPoint(button.checkpoint));
        }
    }
}

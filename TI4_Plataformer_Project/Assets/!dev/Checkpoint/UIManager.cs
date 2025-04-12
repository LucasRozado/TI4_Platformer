using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject fastTravelCanvas;
    [SerializeField] CPButton[] fastTraverlButtons;
    [SerializeField] Button cancelTravel;
    [SerializeField] Button travel;
    [SerializeField] Image travelPreview;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenFastTravel()
    {
        fastTravelCanvas.SetActive(true);
        foreach (CPButton button in fastTraverlButtons)
        {
            button.gameObject.SetActive(CPManager.instance.VerifyCheckPoint(button.checkpoint));
        }
    }

    public void CloseFastTravel()
    {
        travel.gameObject.SetActive(false);
        travelPreview.gameObject.SetActive(false);
        fastTravelCanvas.SetActive(false);
    }

    public void PrepareFastTravel(CPButton button)
    {
        travel.gameObject.SetActive(true);
        travelPreview.gameObject.SetActive(true);
        travelPreview.sprite = button.checkpoint.sceneImage;
        travel.onClick.RemoveAllListeners();
        travel.onClick.AddListener(button.Travel);
    }
}

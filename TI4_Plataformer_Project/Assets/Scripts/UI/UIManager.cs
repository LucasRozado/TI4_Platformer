using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public FastTravelScreen fastTravelScreen;
    public HUD hud;
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
        if (fastTravelScreen == null)
        {
            fastTravelScreen = FindFirstObjectByType<FastTravelScreen>();
        }
        fastTravelScreen.gameObject.SetActive(true);        
    }

    public void OpenPause()
    {
        Time.timeScale = 0;
    }

    public void ClosePause()
    {
        Time.timeScale = 1f;
    }
    
    public void UpdateCollectable(CollectableType type)
    {
        hud.UpdateCollectables(type);
    }

    public void ShowText(string text, float duration)
    {
        hud.DisplayText(text, duration);
    }

}

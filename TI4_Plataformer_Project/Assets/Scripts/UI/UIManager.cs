using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public FastTravelScreen fastTravelScreen;
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
}

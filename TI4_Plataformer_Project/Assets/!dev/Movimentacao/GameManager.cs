using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance => instance;

    private InputSystem_Actions actions;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        actions = new InputSystem_Actions();
    }

    public InputSystem_Actions Actions => actions;
}

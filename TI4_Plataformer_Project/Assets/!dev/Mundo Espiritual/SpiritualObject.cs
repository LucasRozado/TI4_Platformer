using UnityEngine;

public class SpiritualObject : MonoBehaviour
{
    [SerializeField] bool isMundane;

    private void Start()
    {
        SpiritualObserver.instance.Subscribe(this);
        if (!isMundane)
        {
            gameObject.SetActive(false);
        }
    }

    public void SwitchReality(bool isSpiritual)
    {
        if (isSpiritual == isMundane)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

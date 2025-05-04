using System.Collections;
using UnityEngine;

public class ButtonGate : MonoBehaviour
{
    bool isActive;
    [SerializeField] float duration = 3f;
    [SerializeField] float positionDistance;
    Vector3 newPosition;
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        newPosition = transform.position - Vector3.down * positionDistance;
    }
    public void GateOpen()
    {
        if (!isActive)
        { 
            isActive = true;
            StartCoroutine(CRGateOpen()); 
        }
    }

    public IEnumerator CRGateOpen()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, newPosition, t / duration);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}

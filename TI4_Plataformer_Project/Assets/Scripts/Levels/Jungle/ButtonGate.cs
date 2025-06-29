using System.Collections;
using UnityEngine;

public class ButtonGate : ButtonActivated
{
    bool isActive;
    [SerializeField] float duration = 3f;
    [SerializeField] float positionDistance;
    Vector3 newPosition;
    Vector3 startPosition;

    private void OnEnable()
    {
        startPosition = transform.position;
        newPosition = transform.position - Vector3.down * positionDistance;
    }

    public override void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(CRGateOpen());
            //gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public IEnumerator CRGateOpen()
    {
        Debug.Log(gameObject.name);
        startPosition = transform.position;
        Debug.Log(startPosition);
        newPosition = transform.position - Vector3.down * positionDistance;
        Debug.Log(newPosition);
        float t = 0;
        int i = 0;
        while (t < duration)
        {
            i++;
            Debug.Log(i);
            t += Time.deltaTime;
            Debug.Log(t);
            transform.position = Vector3.Lerp(startPosition, newPosition, t / duration);
            Debug.Log(transform.position);
            yield return new WaitForEndOfFrame();
        }
    }

    public override void ManualActivation()
    {
        transform.position = newPosition;
    }
}

using System.Collections;
using UnityEngine;

public class ButtonGate : ButtonActivated
{
    bool isActive;
    [SerializeField] float duration = 3f;
    [SerializeField] float positionDistance;
    Vector3 newPosition;
    Vector3 startPosition;

    public override void Activate()
    {
        if (!isActive)
        {
            Debug.Log("Activate " + gameObject.name);
            isActive = true;
            StartCoroutine(CRGateOpen());
            //gameObject.SetActive(!gameObject.activeSelf);
        }
    }

    public IEnumerator CRGateOpen()
    {
        startPosition = transform.position;
        newPosition = transform.position - Vector3.down * positionDistance;
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

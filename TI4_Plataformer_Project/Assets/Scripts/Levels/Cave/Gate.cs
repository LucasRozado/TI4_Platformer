using System.Collections;
using UnityEngine;

public class Gate : Activated
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
    public override void Activate()
    {
        if (!isActive)
        { 
            isActive = true;
            StartCoroutine(Open_Coroutine()); 
        }
    }

    public IEnumerator Open_Coroutine()
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, newPosition, t / duration);
            yield return null;
        }
    }
}

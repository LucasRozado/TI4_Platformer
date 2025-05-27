using UnityEngine;
using System.Collections;

public class MammothGate : ButtonActivated
{
    [SerializeField] MammothMaze mammoth;
    bool isActive;
    [SerializeField] float duration = 3f;
    [SerializeField] float positionDistance;
    Vector3 newPosition;
    Vector3 startPosition;
    [SerializeField] bool firstGate;

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
            StartCoroutine(CRGateOpen());
            if (firstGate)
            {
                mammoth.firstGate = true;
            }
            else
            {
                mammoth.secondGate = true;
            }
            if (mammoth.firstGate && mammoth.secondGate)
            {
                mammoth.EndMaze();
            }
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

using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class FallingInteractable : Interactable
{
    [SerializeField] float duration;
    [SerializeField] private bool isFixed;
    [SerializeField] bool hasFallen;
    [SerializeField] GameObject[] drops;
    Vector3 pivot;
    public override void InteractWith(Player player)
    {
        if (player.GetPowerUp(PowerUps.Push) && !hasFallen)
        {
            StartCoroutine(Fall());
            hasFallen = true;
            pivot = Vector3.Cross(transform.up, player.transform.forward);
            foreach (GameObject go in drops)
            {
                go.transform.parent = null;
                if (go.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                }
            }
        }
    }

    public IEnumerator Fall()
    {
        float t = 0;
        while (t < duration)
        {
            if (!isFixed)
            {
                t += Time.deltaTime;
                transform.Rotate(pivot.normalized, (90 / duration) * Time.deltaTime);
            }
            else
            {
                t += Time.deltaTime;
                transform.Rotate(transform.right, (90 / duration) * Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class FallingInteractable : Interactable
{
    [SerializeField] float duration;
    [SerializeField] private bool isFixed;
    [SerializeField] bool hasFallen;
    [SerializeField] GameObject[] drops;
    [SerializeField] Animator animator;
    Vector3 pivot;
    public override void InteractWith(Player player)
    {
        if (GameManager.powerUp.GetPowerUp(PowerUps.Push) && !hasFallen)
        {
            //StartCoroutine(Fall());
            if (isFixed)
            {
                hasFallen = true;
            }
            //pivot = Vector3.Cross(transform.up, player.transform.forward);
            else
            {
                foreach (GameObject go in drops)
                {
                    go.transform.parent = null;
                    if (go.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.useGravity = true;
                    }
                }
            }

            animator.SetTrigger("Pushed");
        }
    }

    public IEnumerator Fall()
    {
        Quaternion initialRotation = transform.localRotation;
        Quaternion finalRotation;
        if (isFixed)
        {
            finalRotation = transform.localRotation * Quaternion.AngleAxis(90, Vector3.right);
        }
        else
        {
            finalRotation = transform.rotation * Quaternion.AngleAxis(90, pivot.normalized);
        }
        float t = 0;
        while (t < duration)
        {
            if (isFixed)
            {
                t += Time.deltaTime;
                transform.localRotation = Quaternion.Lerp(initialRotation, finalRotation, t / duration);
            }
            else
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, t / duration);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}

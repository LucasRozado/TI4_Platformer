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
    [SerializeField] int intReference;
    public override void InteractWith(Player player)
    {
        if (GameManager.powerUp.GetPowerUp(PowerUps.Push) && !hasFallen)
        {

            Debug.Log("Falling Interactable Activation");
            //StartCoroutine(Fall());
            if (isFixed)
            {
                hasFallen = true;
                LevelProgress.instance.Activate(intReference);
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

    private void Start()
    {
        if (isFixed && LevelProgress.instance.GetProgress(intReference))
        {
            hasFallen = true;
            animator.SetTrigger("Pushed");
        }
    }
}

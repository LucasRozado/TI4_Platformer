using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float duration;
    private float timer = 0f;
    private int direction = 1;
    [SerializeField] Transform endMovementTarget;
    Vector3 endMovement;
    Vector3 startMovement;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask pushableLayer;
    public Rigidbody player;
    Rigidbody rb;

    private void Start()
    {
        startMovement = transform.position;
        endMovement = endMovementTarget.position;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime * direction;

        Vector3 position = Vector3.Lerp(startMovement, endMovement, timer / duration);


        rb.MovePosition(position);
        if (timer/duration >= 1 || timer/duration <= 0)
        {
            direction *= -1;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Teste");
    }
}

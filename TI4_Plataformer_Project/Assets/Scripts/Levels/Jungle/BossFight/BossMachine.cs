using UnityEngine;

public class BossMachine : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    [SerializeField] protected BossState enterState;
    [SerializeField] protected BossState currentState;
    [SerializeField] int health;
    [SerializeField] int maxHealth;

    public void ChangeState(BossState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        currentState = enterState;
        currentState.EnterState(this);
    }

    public virtual void Start()
    {
        currentState = enterState;
        health = maxHealth;
    }
    public virtual void FixedUpdate()
    {
        currentState.StateFixedUpdate();
    }
    public virtual void Update()
    {
        currentState.StateUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.Trigger(other);
    }
}

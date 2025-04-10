using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Observables")]
    [SerializeField] private PlayerState state;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 forward;
    [SerializeField] private Vector2 movementVelocity;
    [SerializeField] private Vector3 gravityVelocity;
    [SerializeField] private ControllerColliderHit collisionHit;

    [SerializeField] private Transform[] interactChecksLR;
    [SerializeField] private float interactDistance = 0.3f;
    [SerializeField] private LayerMask canInteract;

    private InputSystem_Actions.PlayerActions actions;
    private CharacterController characterController;
    private readonly Dictionary<Type, PlayerState> states = new();
    private void Start()
    {
        actions = GameManager.Instance.Actions.Player;
        actions.Enable();

        forward = transform.forward;

        characterController = GetComponent<CharacterController>();

        state.Enter();
    }

    public Action<ControllerColliderHit, CollisionFlags> collisionUpdate;

    public InputSystem_Actions.PlayerActions Actions => actions;
    public PlayerState State => state;
    public Vector3 Velocity => velocity;
    public Vector3 Forward { get => forward; set => forward = value; }
    public Vector2 Movement { get => movementVelocity; set => movementVelocity = value; }
    public Vector3 Gravity { get => gravityVelocity; set => gravityVelocity = value; }
    public float InteractDistance => interactDistance;
    public LayerMask CanInteract => canInteract;
    public float Slope => characterController.slopeLimit;

    public Transform GetInteractChecks(int i)
    {
        return interactChecksLR[i];
    }

    public void Look(Vector3 forward)
    {
        transform.rotation = Quaternion.LookRotation(forward);
    }

    public void AddState(PlayerState state)
    {
        states[state.GetType()] = state;
        state.enabled = false;
    }

    public T GetState<T>() where T : PlayerState
    {
        T stateInstance = states[typeof(T)] as T;
        return stateInstance;
    }
    public void SwitchState<T>() where T : PlayerState
    {
        PlayerState state = GetState<T>();

        this.state.Enter(state);
        this.state.enabled = false;

        this.state = state;
        this.state.enabled = true;
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;

        CollisionFlags oldCollision = characterController.collisionFlags;
        CollisionFlags newCollision = characterController.Move(velocity * Time.deltaTime);

        bool didCollisionUpdate = collisionHit != null || oldCollision != newCollision;
        if (didCollisionUpdate)
        {
            collisionUpdate?.Invoke(collisionHit, newCollision);
            collisionHit = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        collisionHit = hit;
    }

    private void OnDestroy()
    {
        actions.Disable();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerState[] possibleStates;

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

    private Dictionary<Type, PlayerState> stateInstances;
    private InputSystem_Actions.PlayerActions actions;
    private CharacterController characterController;
    private void Start()
    {
        stateInstances = new();
        foreach (PlayerState state in possibleStates)
        {
            Type stateType = state.GetType();
            PlayerState stateInstance = ScriptableObject.CreateInstance(stateType) as PlayerState;
            stateInstances.Add(stateType, stateInstance);
            stateInstance.Configure(this);
        }

        actions = GameManager.Instance.Actions.Player;
        actions.Enable();

        characterController = GetComponent<CharacterController>();

        if (possibleStates != null && possibleStates.Length > 0)
        {
            PlayerState initialStateDefinition = possibleStates[0];
            PlayerState initialState = stateInstances[initialStateDefinition.GetType()];
            initialState.Enter();
            state = initialState;
        }

        forward = transform.forward;
    }
    
    public Action<ControllerColliderHit, CollisionFlags> collisionUpdate;


    public InputSystem_Actions.PlayerActions Actions => actions;
    public PlayerState State => stateInstances[state.GetType()];
    public Vector3 Velocity => velocity;
    public Vector3 Forward { get => forward; set => forward = value; }
    public Vector2 Movement { get => movementVelocity; set => movementVelocity = value; }
    public Vector3 Gravity { get => gravityVelocity; set => gravityVelocity = value; }
    public float InteractDistance => interactDistance;
    public LayerMask CanInteract => canInteract;

    public Transform GetInteractChecks (int i)
    {
        return interactChecksLR[i];
    }

    public void Look(Vector3 forward)
    {
        transform.rotation = Quaternion.LookRotation(forward);
    }
    public float Slope => characterController.slopeLimit;


    public State GetState<State>() where State : PlayerState
    {
        State stateInstance = stateInstances[typeof(State)] as State;
        return stateInstance;
    }
    public void SwitchState<State>() where State : PlayerState
    {
        PlayerState oldState = stateInstances[state.GetType()];
        PlayerState newState = GetState<State>();

        oldState.Enter(newState);
        state = newState;
    }

    private void Update()
    {
        Vector3 velocity = CalculateVelocity(movementVelocity, gravityVelocity, forward);
        Move(velocity);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        collisionHit = hit;
    }

    private Vector3 CalculateVelocity(Vector2 movement, Vector3 gravity, Vector3 forward)
    {
        velocity = state.CalculateVelocity(movement, gravity, forward);
        return velocity;
    }

    private void Move(Vector3 velocity)
    {
        CollisionFlags oldCollision = characterController.collisionFlags;
        CollisionFlags newCollision = characterController.Move(velocity * Time.deltaTime);

        bool didCollisionUpdate = collisionHit != null || oldCollision != newCollision;
        if (didCollisionUpdate)
        {
            collisionUpdate?.Invoke(collisionHit, newCollision);
            collisionHit = null;
        }
    }
}

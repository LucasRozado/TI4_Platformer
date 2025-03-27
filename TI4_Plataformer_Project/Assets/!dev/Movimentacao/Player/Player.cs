using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Flags]
    public enum Skill
    {
        None = 0,
        Move = 1,
        Jump = 2,
        Climb = 4,
    }


    private PlayerState state;
    private Vector3 velocity;


    [SerializeField] private PlayerState[] possibleStates;
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
            stateInstance.Init(this);
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
    }

    public InputSystem_Actions.PlayerActions Actions => actions;
    public PlayerState State => stateInstances[state.GetType()];
    public Vector3 Velocity => velocity;
    public Vector3 Forward => transform.forward;


    public Action<ControllerColliderHit, CollisionFlags> collided;


    public State GetState<State>() where State : PlayerState
    {
        State stateInstance = stateInstances[typeof(State)] as State;
        return stateInstance;
    }
    public void SwitchState<State>() where State : PlayerState
    {
        PlayerState oldState = stateInstances[state.GetType()];
        PlayerState newState = GetState<State>();

        oldState.Exit();
        newState.Enter();

        state = newState;
    }

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }
    public void AddVelocity(Vector3 velocity)
    {
        this.velocity += velocity;
    }
    public void SetForward(Vector3 forward)
    {
        transform.rotation = Quaternion.LookRotation(forward);
    }

    private CollisionFlags collisionFlags;
    private void Update()
    {
        collisionFlags = characterController.Move(velocity * Time.deltaTime);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        collided?.Invoke(hit, collisionFlags);
    }
}

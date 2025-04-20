using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Transform[] interactChecksLR;
    [SerializeField] private float interactDistance = 0.3f;
    [SerializeField] private LayerMask canInteract;
    public static Player instance;
    [SerializeField] private PlayerState[] possibleStates;

    [Header("Observables")]
    [SerializeField] private PlayerState state;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 forward;
    [SerializeField] private Vector2 movementVelocity;
    [SerializeField] private Vector3 gravityVelocity;

    private PlayerInput input;
    private CharacterController characterController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
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

    private readonly Dictionary<Type, PlayerState> states = new();
    private readonly ControllerCollision lastCollision = new();
    private void Awake()
    {
        forward = transform.forward;

        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        input = new PlayerInput(GameManager.Instance.Actions);
        
        // Inicializando a m�quina de estados
        PlayerState[] states = GetComponents<PlayerState>();
        if (states.Length > 0)
        {
            foreach (PlayerState state in states)
            {
                // Inicializando cada estado
                state.Initialize();

                // Guardando a refer�ncia para cada estado
                this.states[state.GetType()] = state;

                // Iniciando com o primeiro estado marcado como ativo e desabilitando os outros
                if (this.state != null)
                { state.enabled = false; }
                else if (state.enabled)
                { this.state = state; }
            }

            // Se nenhum estado estava ativo, inicia com o primeiro da lista
            if (this.state != null)
            {
                this.state = states[0];
                this.state.enabled = true;
            }

            // Iniciando o primeiro estado
            state.Enter();
        }
        else
        { this.state = null; }
    }

    public Action<ControllerColliderHit, CollisionFlags> collisionUpdate;

    public PlayerInput Input => input;
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

    public ControllerCollision Move(Vector3 velocity)
    {
        this.velocity = velocity;

        CollisionFlags oldCollisionFlags = characterController.collisionFlags;
        CollisionFlags newCollisionFlags = characterController.Move(velocity * Time.deltaTime);
        // [OnControllerColliderHit] � chamado no [Move] caso haja colis�o
        lastCollision.flags = newCollisionFlags;

        return lastCollision;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastCollision.hit = hit;
    }

    public class ControllerCollision
    {
        public CollisionFlags flags;
        public ControllerColliderHit hit;
    }

    public void ToggleController(bool toggle)
    {
        characterController.enabled = toggle;
    }
}

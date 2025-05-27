using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using UnityEngine.InputSystem.LowLevel;
using Unity.Cinemachine;
using System.Collections;


[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance;

    [Header("Stats")]
    [SerializeField] private int hpBase = 2;

    [Header("Interaction")]
    [SerializeField] private Transform[] interactChecksLR;
    [SerializeField] private float interactDistance = 0.3f;
    [SerializeField] private LayerMask canInteract;

    [Header("PowerUps")]
    [SerializeField] private bool[] hasPowerUp = new bool[4];

    [Header("Observables")]
    [SerializeField] private int hpCurrent;
    [SerializeField] private PlayerState state;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 forward;
    [SerializeField] private Vector2 movementVelocity;
    [SerializeField] private Vector3 gravityVelocity;

    private PlayerInput input;
    private CharacterController characterController;
    private readonly Dictionary<Type, PlayerState> states = new();
    private readonly ControllerCollision lastCollision = new();
    private ControllerColliderHit collisionHitBuffer;

    private PlayerAnimations playerAnimations;
    private void Awake()
    {
        if (instance == null)
        { instance = this; }
        else
        { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        hpCurrent = hpBase;
        forward = transform.forward;
        characterController = GetComponent<CharacterController>();
        playerAnimations = GetComponent<PlayerAnimations>();
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
                // Guardando a referencia para cada estado
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
            }

            // Inicializando os estados
            // (Precisa ser em outro loop para caso os estados precisem acessar uns aos outros)
            foreach (PlayerState state in states)
            { state.Initialize(); }

            // Iniciando o primeiro estado
            state.Enter();
        }
        else
        { this.state = null; }

    }

    public Action<ControllerColliderHit, CollisionFlags> collisionUpdate;

    public PlayerAnimations PlayerAnimations => playerAnimations;
    public PlayerInput Input => input;
    public PlayerState State => state;
    public Vector3 Velocity => velocity;
    public Vector3 Forward { get => forward; set => forward = value; }
    public Vector2 Movement { get => movementVelocity; set => movementVelocity = value; }
    public Vector3 Gravity { get => gravityVelocity; set => gravityVelocity = value; }
    public float InteractDistance => interactDistance;
    public LayerMask CanInteract => canInteract;
    public float Slope => characterController.slopeLimit;

    public Transform LeftInteractionChecker => interactChecksLR[0];
    public Transform RightInteractionChecker => interactChecksLR[1];
    public Transform GetInteractChecks(int i)
    {
        return interactChecksLR[i];
    }

    public void Look(Quaternion forward)
    {
        transform.rotation = forward;
    }
    public void Look(Vector3 forward)
    {
        Look(Quaternion.LookRotation(forward));
    }

    public T GetState<T>() where T : PlayerState
    {
        T stateInstance = states[typeof(T)] as T;
        return stateInstance;
    }
    public void GetState<T>(out T state) where T : PlayerState
    {
        state = GetState<T>();
    }
    public void SwitchState<T>() where T : PlayerState
    {
        PlayerState state = GetState<T>();
        SwitchState(state);
    }
    public void SwitchState(PlayerState state)
    {
        this.state.Exit();
        this.state = state;
        this.state.Enter();
    }

    public ControllerCollision Move(Vector3 velocity)
    {
        this.velocity = velocity;

        CollisionFlags collisionFlags = characterController.Move(velocity * Time.deltaTime);
        // [OnControllerColliderHit] � chamado no [Move] caso haja colis�o
        lastCollision.flags = collisionFlags;

        return lastCollision;
    }
    public delegate void CollisionHandler(CollisionFlags flags, ControllerColliderHit hit);
    public void Move(Vector3 velocity, CollisionHandler onFlagsUpdate = null, CollisionHandler onCollision = null)
    {
        this.velocity = velocity;

        CollisionFlags lastCollisionFlags = characterController.collisionFlags;
        CollisionFlags collisionFlags = characterController.Move(velocity * Time.deltaTime);
        // [OnControllerColliderHit] eh chamado no [Move] caso haja colisao

        if (lastCollisionFlags != collisionFlags && onFlagsUpdate != null)
        {
            onFlagsUpdate(collisionFlags, collisionHitBuffer);
        }
        else if (collisionHitBuffer != null && onCollision != null)
        {
            onCollision(collisionFlags, collisionHitBuffer);
            collisionHitBuffer = null;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        lastCollision.hit = hit;
        collisionHitBuffer = hit;
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

    [Obsolete("Use GameManager.powerUp.GetPowerUp")]
    public bool GetPowerUp(PowerUps type)
    {
        return hasPowerUp[(int)type];
    }
    [Obsolete("Use GameManager.powerUp.AcquirePowerUp")]
    public void AcquirePowerUp(PowerUps type)
    {
        hasPowerUp[(int)type] = true;
    }

    public void TakeDamage()
    {
        hpCurrent -= 1;
        if (hpCurrent == 0)
        { Die(); }
    }

    public void Heal()
    {
        hpCurrent = hpBase;
    }

    public void Die()
    {
        // TODO: Animation
        StartCoroutine(Die_Coroutine());
    }
    private IEnumerator Die_Coroutine()
    {
        const float respawnDelay = 2f; // TODO: por a duracao da animacao de morte

        yield return new WaitForSeconds(respawnDelay);

        // TODO: animacao de respawn (a mesma de marcar checkpoint, se tiver)

        GameManager.Instance.ResetToCheckPoint();
    }
}

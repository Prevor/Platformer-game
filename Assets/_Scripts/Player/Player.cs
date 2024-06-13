using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int _health = 80;
    public int _maxHealth = 100;
    [SerializeField] private Animator _animator;
    [SerializeField] private Crossbow _crossbow;
    [SerializeField] private GameObject _hurtBox;

    //Player State
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    //Player Component
    public PlayerController PlayerController { get; private set; }
    public Animator Animator { get; private set; }
    public Crossbow Crossbow { get; private set; }
    public CharacterController CharacterController { get; internal set; }



    public float knockbackForce = 5f; // Adjust this to your liking
    public float knockbackDuration = 0.2f; // Adjust this to your liking
    public float knockbackDecay = 5f; // Adjust this to your liking

    private Vector3 knockbackVelocity;
    private Vector3 knockbackDirection = Vector3.up;
    [SerializeField] private bool isDamage;

    private void Awake()
    {

        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, "IsIdle");
        MoveState = new PlayerMoveState(this, StateMachine, "IsRunning");
        JumpState = new PlayerJumpState(this, StateMachine, "IsJumping");
        AirState = new PlayerAirState(this, StateMachine, "IsAir");
        LandState = new PlayerLandState(this, StateMachine, "IsLand");
        AttackState = new PlayerAttackState(this, StateMachine, "IsAttack");
        ShootState = new PlayerShootState(this, StateMachine, "IsShooting", "IsShoot");

    }
    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        Animator = _animator;
        Crossbow = _crossbow;
        CharacterController = GetComponent<CharacterController>();
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        if (knockbackVelocity != Vector3.zero)
        {
            knockbackVelocity -= knockbackVelocity * knockbackDecay * Time.deltaTime;
            if (knockbackVelocity.magnitude < 0.1f) // To avoid very small values causing jitter
            {
                knockbackVelocity = Vector3.zero;
            }
        }
    }
    private void FixedUpdate()
    {
        PlayerController.Land();
        StateMachine.CurrentState.PhysicsUpdate();

        CharacterController.Move(knockbackVelocity);
    }

    public void ApplyDamage(DamageType damageType, int damage, Vector3 direction)
    {
        _health -= damage;
        isDamage = true;
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            knockbackVelocity = (direction+new Vector3(0,0.5f,0)) * knockbackForce * Time.deltaTime; // Set the knockback velocity
        }
    }

    private void Die()
    {
        Debug.Log("Game Over");
    }
}

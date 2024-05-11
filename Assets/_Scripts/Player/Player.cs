using UnityEngine;

public class Player : MonoBehaviour
{
    public int _health = 80;
    public int _maxHealth = 100;
    [SerializeField] private Animator _animator;
    [SerializeField] private Crossbow _crossbow;
    [SerializeField] private Transform _playerCamera;

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
    public Transform PlayerCamera { get; private set; }

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
        PlayerCamera = _playerCamera;
        CharacterController = GetComponent<CharacterController>();
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        PlayerController.Land();
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void ApplyDamage(DamageType damageType, int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Game Over");
    }
}

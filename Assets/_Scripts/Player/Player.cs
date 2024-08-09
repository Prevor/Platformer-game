using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private Crossbow _crossbow;
    [SerializeField] private int _indexWeapon;
    [SerializeField] private GameObject[] _weapons;


    //Player State
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAirState AirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerShootState ShootState { get; private set; }
    public PlayerGetHitState GetHitState { get; private set; }
    public PlayerDeadState DeadState { get; private set; }

    //Player Component
    public PlayerController PlayerController { get; private set; }
    public Animator Animator { get; private set; }
    public Crossbow Crossbow { get; private set; }
    public CharacterController CharacterController { get; internal set; }
    public Transform PlayerCamera { get; private set; }
    public TargetSelectPlayer TargetSelectPlayer { get; private set; }

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
        GetHitState = new PlayerGetHitState(this, StateMachine, "IsGetingHit");
        DeadState = new PlayerDeadState(this, StateMachine, "IsDead");
    }
    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        CharacterController = GetComponent<CharacterController>();
        Animator = _animator;
        PlayerCamera = _playerCamera;
        Crossbow = _crossbow;
        StateMachine.Initialize(IdleState);
        TargetSelectPlayer = GetComponent<TargetSelectPlayer>();
        SelectedWeapon(_indexWeapon);
    }
    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    private void SelectedWeapon(int weaponIndex)
    {
        _weapons[weaponIndex - 1].SetActive(true);
    }
}

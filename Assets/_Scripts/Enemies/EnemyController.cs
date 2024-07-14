using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _pointNav;
    [SerializeField] private Transform _hitBox;
    

   // [SerializeField] private float _hitBoxRadius;
    [SerializeField] private float _radiusAttack;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _radiusDetection;
    [SerializeField] private float _radiusPatrol;

    [SerializeField] private bool _isDistanceAttack;
    [SerializeField] private bool _isAttack;
    [SerializeField] private bool _isChase;

    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private EnemyAttack _detectionAttack;

    private float _distance;
    private Animator _animator;
    private NavMeshAgent _navMesh;

    public EnemyStateMachine StateMachine { get; private set; }
    public IdleState IdleState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public PatrolState PatrolState { get; private set; }
    public AttackState AttackState { get; private set; }
    
    public Animator Animator { get => _animator; private set => _animator = value; }
    public Transform PointNav { get => _pointNav; set => _pointNav = value; }
    public NavMeshAgent NavMesh { get => _navMesh; set => _navMesh = value; }
    public PlayerController _playerController;

    //public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public bool IsChase { get => _isChase; set => _isChase = value; }
   // public float HitBoxSize { get => _hitBoxRadius; set => _hitBoxRadius = value; }
    public float Distance { get => _distance; set => _distance = value; }
    public float RayDistance { get => _rayDistance; set => _rayDistance = value; }
    //public LayerMask DetectionLayer { get => _detectionLayer; set => _detectionLayer = value; }
    public Vector3 Direction { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMesh = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        IdleState = new IdleState(this, StateMachine, "IsIdle");
        ChaseState = new ChaseState(this, StateMachine, "IsMove", _radiusAttack);
        PatrolState = new PatrolState(this, StateMachine, "IsMove", _radiusPatrol);
        AttackState = new AttackState(this, StateMachine, "IsIdleBattel", "IsAttack", _isDistanceAttack, _hitBox);

    }
    private void Start()
    {
        //_playerPosition = PlayerManager.instance.Player.transform;
        // _player = 
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, _radiusDetection, _detectionLayer.value))
        {
            _distance = Vector3.Distance(transform.position, _playerController.PlayerPosition.position);
            _isChase = true;
        }
        else
        {
            _isChase = false;
        }

        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    } 
    public void FaceTarget()
    {
        Direction = (_playerController.PlayerPosition.position - transform.position).normalized;
        Quaternion lookRptation = Quaternion.LookRotation(new Vector3(Direction.x, 0, Direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRptation, Time.deltaTime * 5f);
    }
    public bool IsEnemyAttack()
    {
       return Physics.CheckSphere(transform.position, _radiusAttack, _detectionLayer.value) && _playerController._playerHealth.Health != 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(_pointNav.position, _radiusPatrol);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusDetection);
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_hitBox.position, _hitBox.forward * _rayDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radiusAttack);  
    }
}

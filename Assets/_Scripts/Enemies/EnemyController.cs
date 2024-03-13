using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _pointNav;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private Transform _hitBox;

    [SerializeField] private float _hitBoxRadius;
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
    public Transform PlayerPosition { get => _playerPosition; set => _playerPosition = value; }
  
    public bool IsAttack { get => _isAttack; set => _isAttack = value; }
    public bool IsChase { get => _isChase; set => _isChase = value; }
    public float HitBoxRadius { get => _hitBoxRadius; set => _hitBoxRadius = value; }
    public float Distance { get => _distance; set => _distance = value; }

    private void OnEnable()
    {
       
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMesh = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        IdleState = new IdleState(this, StateMachine, "IsIdle");
        ChaseState = new ChaseState(this, StateMachine, "IsMove", _navMesh);
        PatrolState = new PatrolState(this, StateMachine, "IsMove", _radiusPatrol);
        AttackState = new AttackState(this, StateMachine, "IsIdleBattel", "IsAttack", _isDistanceAttack, _hitBox);

    }
    private void Start()
    {
        _playerPosition = PlayerManager.instance.Player.transform;
        StateMachine.Initialize(IdleState);
    }
    private void Update()
    {
      
        if (Physics.CheckSphere(transform.position, _radiusDetection, _detectionLayer.value))
        {
            _distance = Vector3.Distance(transform.position, _playerPosition.position);
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
        Vector3 direction = (_playerPosition.position - transform.position).normalized;
        Quaternion lookRptation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRptation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawWireSphere(_pointNav.position, _radiusPatrol);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusDetection); 
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_hitBox.position, _hitBoxRadius);
    }
}

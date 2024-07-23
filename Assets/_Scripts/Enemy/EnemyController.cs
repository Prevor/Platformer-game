using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform _pointNav;
    [SerializeField] private Transform _hitBox;

    [SerializeField] private float _radiusAttack;
    [SerializeField] private float _radiusDistanceAttack;
    [SerializeField] private float _rayDistance;
    [SerializeField] private float _radiusDetection;
    [SerializeField] private float _radiusPatrol;

    [SerializeField] private bool _isDistanceAttack;
    [SerializeField] private bool _isAttack;
    [SerializeField] private bool _isChase;

    [SerializeField] private LayerMask _detectionLayer;
    [SerializeField] private EnemyAttack _detectionAttack;

    [SerializeField] private SpiderWeb _prefabSpiderWeb;
    [SerializeField] private Transform _castPoint;

    private float _radiusDistanceAttackTemp;
    private float _distance;
    private Animator _animator;
    private NavMeshAgent _navMesh;

    public EnemyStateMachine StateMachine { get; private set; }
    public IdleState IdleState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public PatrolState PatrolState { get; private set; }
    public AttackState AttackState { get; private set; }
    public ShootState ShootState { get; private set; }

    public Animator Animator { get => _animator; private set => _animator = value; }
    public Transform PointNav { get => _pointNav; set => _pointNav = value; }
    public NavMeshAgent NavMesh { get => _navMesh; set => _navMesh = value; }
    public PlayerController _playerController;


    public bool IsChase { get => _isChase; set => _isChase = value; }
    public float Distance { get => _distance; set => _distance = value; }
    public float RayDistance { get => _rayDistance; set => _rayDistance = value; }
    public Vector3 Direction { get; private set; }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navMesh = GetComponent<NavMeshAgent>();
        StateMachine = new EnemyStateMachine();
        IdleState = new IdleState(this, StateMachine, "IsIdle");
        ChaseState = new ChaseState(this, StateMachine, "IsMove", _radiusAttack);
        PatrolState = new PatrolState(this, StateMachine, "IsMove", _radiusPatrol);
        AttackState = new AttackState(this, StateMachine, "IsIdleBattel", "IsAttack", _hitBox);
        ShootState = new ShootState(this, StateMachine, "IsIdleBattel", "IsSoot", _radiusDetection, _isDistanceAttack);

    }
    private void Start()
    {
        StateMachine.Initialize(IdleState);

        _radiusDistanceAttackTemp = _radiusDistanceAttack;
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
        return Physics.CheckSphere(transform.position, _radiusAttack, _detectionLayer.value) 
            && _playerController._playerHealth.Health != 0;
        
    }

    public bool IsDistanceAttack()
    {
        return !IsEnemyAttack() 
            && !_isChase
            && _isDistanceAttack
            && _playerController._playerHealth.Health != 0
            && Physics.CheckSphere(transform.position, _radiusDistanceAttack, _detectionLayer.value);
    }


    public void Cast()
    {
        SpiderWeb _spiderWeb = ObjectPool.Instance.GetObject(_prefabSpiderWeb);
        _spiderWeb.transform.position = _castPoint.transform.position;
        _spiderWeb.transform.rotation = _castPoint.transform.rotation;

        _spiderWeb.FlyInDirection(transform, _playerController.transform);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radiusDistanceAttack);
    }
}

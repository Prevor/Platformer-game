public class ChaseState : EnemyState
{
    private float _radiusAttack;

    public ChaseState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, float radiusAttack) : base(enemy, stateMachine, animBoolName)
    {
        _radiusAttack = radiusAttack;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (!EnemyController.IsChase)
        {
            StateMachine.ChangeState(EnemyController.IdleState);
        }
        else if (EnemyController.IsEnemyAttack())
        {
            StateMachine.ChangeState(EnemyController.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (EnemyController.Distance >= EnemyController.RayDistance)
        {
            EnemyController.NavMesh.SetDestination(EnemyController._playerController.PlayerPosition.position);
        }

        EnemyController.FaceTarget();
    }

}

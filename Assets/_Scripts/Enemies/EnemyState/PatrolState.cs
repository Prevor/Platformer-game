using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{

    private float _radiusPatrol;
    private Vector3 _lastPosition;
    private Vector3 _targetPosition;
    public PatrolState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, float radiusPatrol) : base(enemy, stateMachine, animBoolName)
    {
        _radiusPatrol = radiusPatrol;
    }

    public override void Enter()
    {
        _lastPosition = EnemyController.transform.position;
        MoveToRandomPosition();
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (EnemyController.NavMesh.remainingDistance <= 2f)
        {
            StateMachine.ChangeState(EnemyController.IdleState);
        }
        if (EnemyController.IsChase)
        {
            StateMachine.ChangeState(EnemyController.ChaseState);

        }
    }

    public override void PhysicsUpdate()
    {

    }
    private void MoveToRandomPosition()
    {
        do
        {
            Vector3 direction = Random.insideUnitSphere * _radiusPatrol;
            direction.y = 0;
            _targetPosition = EnemyController.PointNav.position + direction;
        }
        while (Vector3.Distance(_targetPosition, _lastPosition) < 2f);

        EnemyController.NavMesh.SetDestination(_targetPosition);
        _lastPosition = _targetPosition;
    }
}

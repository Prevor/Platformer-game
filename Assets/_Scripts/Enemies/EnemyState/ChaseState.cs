using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyState
{
    private NavMeshAgent _navMesh;
    private float _distance;
    public ChaseState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, NavMeshAgent navMeshAgent) : base(enemy, stateMachine, animBoolName)
    {
        _navMesh = navMeshAgent;
    }

    public override void Enter()
    {
        // Debug.Log("ChaseState");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        EnemyController.NavMesh.SetDestination(EnemyController.PlayerPosition.position);

        EnemyController.FaceTarget();
        if (EnemyController.Distance <= 1.6f)
        {
          //  EnemyController.IsAttack = true;
            StateMachine.ChangeState(EnemyController.AttackState);

        }
        if (!EnemyController.IsChase)
        {
            StateMachine.ChangeState(EnemyController.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {

    }
   
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : EnemyState
{
    private float _timer = 0;
    private float _timeIdle;
    public IdleState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
       
        _timeIdle = Random.Range(0.5f, 2f);
        base.Enter();
    }

    public override void Exit()
    {
        _timer = 0;
        base.Exit();
    }

    public override void LogicUpdate() 
    {
        _timer += Time.deltaTime;

        if (_timer >= _timeIdle)
        {
            StateMachine.ChangeState(EnemyController.PatrolState);
        }
        else if (EnemyController.IsChase)
        {
            StateMachine.ChangeState(EnemyController.ChaseState);
        }
    }



    public override void PhysicsUpdate()
    {

    }
}


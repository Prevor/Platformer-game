using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController EnemyController;
    protected EnemyStateMachine StateMachine;

    protected float StartTime;
    private string _animBoolName;

    public EnemyState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName)
    {
        EnemyController = enemy;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
		
        EnemyController.Animator.SetBool(_animBoolName, true);
    }
    public virtual void Exit()
    {
        EnemyController.Animator.SetBool(_animBoolName, false);
    }
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
}

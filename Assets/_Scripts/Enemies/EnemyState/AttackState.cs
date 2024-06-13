using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class AttackState : EnemyState
{
    private string _animTrigerName;
    private Transform _hitBox;
    private float _timeAttack;

    public AttackState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, string animTrigerName, bool isDistanceAttack, Transform hitBox) : base(enemy, stateMachine, animBoolName)
    {
        _animTrigerName = animTrigerName;
        _hitBox = hitBox;
    }

    public override void Enter()
    {
        Attack();
        base.Enter();
    }

    public override void Exit()
    {
        _timeAttack = 0;
        //EnemyController.IsAttack = false;
        base.Exit();
    }

    public override void LogicUpdate()
    {

        
        _timeAttack += Time.deltaTime;

        if (!EnemyController.IsEnemyAttack())
        {
            StateMachine.ChangeState(EnemyController.IdleState);
        }
     
    }

    public override void PhysicsUpdate()
    {
        EnemyController.FaceTarget();
        if (_timeAttack > 0.4f)
        {
            Attack();
            _timeAttack = 0;
        }
    }

    private void Attack()
    {
        EnemyController.Animator.SetTrigger(_animTrigerName);
    }
    
}

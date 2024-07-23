using UnityEngine;

public class ShootState : EnemyState
{
    string _animBoolName;
    string _animTrigerName;
    private float _timeShoot;


    public ShootState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, string animTrigerName, float radiusAttack, bool isDistanceAatack ) : base(enemy, stateMachine, animBoolName)
    {
        _animBoolName = animBoolName;
        _animTrigerName = animTrigerName;
    }

    public override void Enter()
    {
        base.Enter();
        Shoot();
    }

    public override void Exit()
    {
        _timeShoot = 0;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        _timeShoot += Time.deltaTime;


        if (!EnemyController.IsDistanceAttack())
        {
            StateMachine.ChangeState(EnemyController.IdleState);
        }
        //else if (EnemyController.IsChase)
        //{
        //    StateMachine.ChangeState(EnemyController.ChaseState);
        //}
        else if (EnemyController.IsEnemyAttack())
        {
            StateMachine.ChangeState(EnemyController.AttackState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        EnemyController.FaceTarget();
        if (_timeShoot > 1f)
        {
            Shoot();
            _timeShoot = 0;
        }
    }

    private void Shoot()
    {
        EnemyController.Animator.SetTrigger(_animTrigerName);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootState : PlayerState
{
    private float _lastShootTime;
    private float _shootEnd = 1f;
    private Vector2 _inputDirection;
    private string _animTriggerName;
    public PlayerShootState(Player player, PlayerStateMachine stateMachine, string animBoolName, string animTriggerName) : base(player, stateMachine, animBoolName)
    {
        _animTriggerName = animTriggerName;
    }

    public override void Enter()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        base.Enter();
        Player.StartCoroutine(ShootDelay(0.4f));
        Player.PlayerController.PlayerShoot += Shoot;
        _lastShootTime = 0;

    }

    public override void Exit()
    {
        Player.PlayerController.ResetInput();

        Player.PlayerController.PlayerShoot -= Shoot;

        base.Exit();
       
    }

    public override void LogicUpdate()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        _lastShootTime += Time.deltaTime;
        if (_lastShootTime > _shootEnd)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (Player.PlayerController.InputDirection != Vector2.zero && _lastShootTime > _shootEnd)
        {
            StateMachine.ChangeState(Player.MoveState);
        }
        else if (Player.PlayerController.IsJumping)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
        else if (Player.PlayerController.IsAttack)
        {
            StateMachine.ChangeState(Player.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        ForwardShoot();
    }

    private void ForwardShoot()
    {
        Vector3 movementDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        if (movementDirection != Vector3.zero)
        {
            Player.transform.forward = movementDirection;
        
        }

    }

    private void Shoot()
    {
        _lastShootTime = 0;
        Player.Crossbow.Cast();
        Player.Animator.SetTrigger(_animTriggerName);
    }
    private IEnumerator ShootDelay(float time)
    {
                
        yield return new WaitForSeconds(time);
        Shoot();
    }

}

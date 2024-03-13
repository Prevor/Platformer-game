using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMoveState : PlayerState
{
    private Vector2 _inputDirection;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        base.Enter();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void LogicUpdate()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        if (_inputDirection == Vector2.zero)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (Player.PlayerController.IsJumping)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
        else if(!Player.PlayerController.IsGrounded && Player.PlayerController.PlayerVelocity.y < -0.3f)
        {
            StateMachine.ChangeState(Player.AirState);
        }
        else if (Player.PlayerController.IsShooting)
        {
            StateMachine.ChangeState(Player.ShootState);
        }

    }
    public override void PhysicsUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 movementDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y);
        Player.CharacterController.Move(movementDirection * Player.PlayerController.PlayerSpeed * Time.fixedDeltaTime);

        if (movementDirection != Vector3.zero)
        {
            Player.transform.forward = movementDirection;
        }

    }
}

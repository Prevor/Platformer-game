using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private Vector2 _inputDirection;

    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        base.Enter();
    }

    public override void Exit()
    {
        _inputDirection = Vector2.zero;
        base.Exit();
    }

    public override void LogicUpdate()
    {
        _inputDirection = Player.PlayerController.InputDirection;
        if (Player.PlayerController.IsGrounded)
        {
            StateMachine.ChangeState(Player.LandState);
        }
        else if (Player.PlayerController.IsDoubleJumping)
        {
            Player.PlayerController.ResetInput();
            StateMachine.ChangeState(Player.JumpState);
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

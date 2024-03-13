using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

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
        if (Player.PlayerController.InputDirection != Vector2.zero && Player.PlayerController.IsGrounded)
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
        else if (Player.PlayerController.IsShooting)
        {
            StateMachine.ChangeState(Player.ShootState);
        }
    }

    public override void PhysicsUpdate()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitState : PlayerState
{
    public PlayerGetHitState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        Player.PlayerController.IsGetingHit = false;
    }

    public override void LogicUpdate() 
    {
        if (Player.PlayerController.IsGetingHit)
        {
            StateMachine.ChangeState(Player.GetHitState);
            
        }
        if (Player.PlayerController.IsDead)
        {
            StateMachine.ChangeState(Player.DeadState);
        }
        else
        {
            StateMachine.ChangeState(Player.IdleState);
        }
    }

    public override void PhysicsUpdate() { }
}

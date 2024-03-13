using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
        Player.Animator.SetInteger("CurrentCombo", Player.PlayerController.CurrentCombo);
        if (!Player.PlayerController.IsAttack || Player.PlayerController.ComboEnd)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {

    }
}

using UnityEngine;

public class PlayerLandState : PlayerState
{
    private float _timerLand = 0.2f;
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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
        _timerLand -= Time.deltaTime;
        if (_timerLand <= 0)
        {
            _timerLand = 0.2f;
            Player.PlayerController.ResetInput();
            if (Player.PlayerController.InputDirection != Vector2.zero)
            {
                StateMachine.ChangeState(Player.MoveState);
            }
            else if (Player.PlayerController.InputDirection == Vector2.zero)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
    }

    public override void PhysicsUpdate()
    {

    }

}

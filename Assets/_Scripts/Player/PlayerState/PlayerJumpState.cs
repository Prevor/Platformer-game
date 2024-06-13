public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Player.PlayerController.Jump();
    }

    public override void Exit()
    {

        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (!Player.PlayerController.IsGrounded)
        {
            StateMachine.ChangeState(Player.AirState);
        }
        else if (Player.PlayerController.IsDoubleJumping)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {

    }


}

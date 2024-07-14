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
        if (!Player.PlayerController.IsAttack)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        //else if (Player.PlayerController.)
        //{

        //}

    }

    public override void PhysicsUpdate()
    {

    }
}

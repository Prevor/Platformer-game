public abstract class PlayerState
{
    protected Player Player;
    protected PlayerStateMachine StateMachine;

    protected float StartTime;
    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        Player = player;
        StateMachine = stateMachine;
        _animBoolName = animBoolName;
    }
    public virtual void Enter()
    {
        Player.Animator.SetBool(_animBoolName, true);
    }
    public virtual void Exit()
    {
        Player.Animator.SetBool(_animBoolName, false);
    }
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
}



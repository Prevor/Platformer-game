using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private Vector3 _inputDirection;
    private float turnSmoothVelocity;


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
        if (_inputDirection == Vector3.zero)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (Player.PlayerController.IsJumping)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
        else if (!Player.PlayerController.IsGrounded && Player.PlayerController.PlayerVelocity.y < -0.3f)
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
        Vector3 movementDirection = new Vector3(_inputDirection.x, 0, _inputDirection.y).normalized;

        if (movementDirection != Vector3.zero)
        {
            //Turns the player in the right direction
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + Player.PlayerCamera.eulerAngles.y;
            float angleSmooth = Mathf.SmoothDampAngle(Player.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.15f);
            Player.transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);

            // Move player
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Player.CharacterController.Move(moveDir.normalized * Player.PlayerController.PlayerSpeed * Time.fixedDeltaTime);

        }

    }
}

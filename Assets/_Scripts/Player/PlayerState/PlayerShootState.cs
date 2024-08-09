using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class PlayerShootState : PlayerState
{
    private float _lastShootTime;
    private float _shootEnd = 1f;
    private Vector2 _inputDirection;
    private string _animTriggerName;
    private Vector3 _directionArrow;
    private Quaternion _croosbowRotation;

    public PlayerShootState(Player player, PlayerStateMachine stateMachine, string animBoolName, string animTriggerName) : base(player, stateMachine, animBoolName)
    {
        _animTriggerName = animTriggerName;
    }

    public override void Enter()
    {

        _inputDirection = Player.PlayerController.InputDirection;
        _croosbowRotation = Player.Crossbow.transform.rotation;
        base.Enter();
        Player.StartCoroutine(ShootDelay(0.4f));
        Player.PlayerController.PlayerShoot += Shoot;
        _lastShootTime = 0;
    }

    public override void Exit()
    {
        Player.PlayerController.ResetInput();

        Player.PlayerController.PlayerShoot -= Shoot;

        base.Exit();

    }

    public override void LogicUpdate()
    {

        _inputDirection = Player.PlayerController.InputDirection;
        _lastShootTime += Time.deltaTime;
        Player.TargetSelectPlayer.FindEnemy();

        if (_lastShootTime > _shootEnd)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (Player.PlayerController.InputDirection != Vector2.zero && _lastShootTime > _shootEnd)
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
    }

    public override void PhysicsUpdate()
    {
        ForwardShoot();
    }

    private void ForwardShoot()
    {
        if (Player.TargetSelectPlayer.CurrentEnemy != null)
        {
            Vector3 playerToEnemy = Player.TargetSelectPlayer.CurrentEnemy.position - Player.transform.position;
            playerToEnemy.y = 0;

            Quaternion targetRotationForPlayer = Quaternion.LookRotation(playerToEnemy);
            Player.transform.rotation = Quaternion.Slerp(Player.transform.rotation, targetRotationForPlayer, 1f);
        }
    }

    private void Shoot()
    {
        _lastShootTime = 0;
        Player.Crossbow.Cast(Player.TargetSelectPlayer.CurrentEnemy);
        Player.Animator.SetTrigger(_animTriggerName);
    }

    private IEnumerator ShootDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Shoot();
    }
    void AimToEnemy(Transform enemyTrs)
    {
        var playerToMouse = enemyTrs.position - Player.transform.position;
        playerToMouse.y = 0f;

        Player.transform.forward = Vector3.Lerp(Player.transform.forward, playerToMouse, 1f);
    }
}

using System.Collections;
using UnityEngine;


public class AttackState : EnemyState
{
    private bool _isDistanceAttack;
    private string _animTrigerName;
    private float _distance;
    private Transform _hitBox;
    Ray hit;
    private float _timeAttack;

    public AttackState(EnemyController enemy, EnemyStateMachine stateMachine, string animBoolName, string animTrigerName, bool isDistanceAttack, Transform hitBox) : base(enemy, stateMachine, animBoolName)
    {
        _isDistanceAttack = isDistanceAttack;
        _animTrigerName = animTrigerName;
        _hitBox = hitBox;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        _timeAttack = 0;

        base.Exit();
    }

    public override void LogicUpdate()
    {

        Debug.DrawRay(_hitBox.position, _hitBox.forward * 0.2f, Color.yellow);

        EnemyController.FaceTarget();
        _timeAttack += Time.deltaTime;

        if (EnemyController.Distance >= 1.6f)
        {
            //      EnemyController.IsAttack = false;
            StateMachine.ChangeState(EnemyController.ChaseState);

        }
        if (_timeAttack > 1f)
        {
            _timeAttack = 0;
            Attack();
        }

    }

    public override void PhysicsUpdate()
    {

    }

    private void Attack()
    {
     //   RaycastHit hit;
        //
        //зменшити дальність рейкасту
        //
        if (Physics.Raycast(_hitBox.position, _hitBox.forward, 5f))
        {

            EnemyController.Animator.SetTrigger(_animTrigerName);
            Collider[] hitColliders = Physics.OverlapSphere(_hitBox.position, EnemyController.HitBoxRadius);
            if (hitColliders != null)
            {
                if (hitColliders[0].TryGetComponent<Player>(out Player damage))
                {
                    EnemyController.StartCoroutine(AttackTimeOut(damage));
                    // damage.gameObject.transform.position += Vector3.back*5f;

                }
            }

        }


        /*if (Physics.Raycast(_hitBox.position, _hitBox.forward, 5f))
        {
            Debug.Log("true");
            if (Physics.Raycast(_hitBox.position, _hitBox.forward, out hit, 0.2f))
            {


                if (hit.transform.gameObject.TryGetComponent<Player>(out Player damage))
                {
                    EnemyController.StartCoroutine(AttackTimeOut(damage));
                    // damage.gameObject.transform.position += Vector3.back*5f;

                }
            }
        }*/

    }
    public IEnumerator AttackTimeOut(Player damage)
    {
        yield return new WaitForSeconds(0.4f);
   //     damage.TakeDamage(DamageType.Physical, 10);

    }
}

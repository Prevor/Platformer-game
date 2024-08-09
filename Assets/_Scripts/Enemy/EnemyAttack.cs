using Tripolygon.UModeler.UI.Input;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private float _forceKnockBack;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            Debug.Log("Successesfuly attack Player by spider");
            Vector3 directionKnockBack = (other.transform.position - transform.position).normalized;

            damage.TakeDamage(DamageType.Physical, 10);
            damage.KnocBack(directionKnockBack, _forceKnockBack);
            _hitEffect.Play();
        }
    }
}

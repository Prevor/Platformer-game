using Tripolygon.UModeler.UI.Input;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private ParticleSystem _hitEffect;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            Debug.Log("Successesfuly attack Player by spider");
            damage.TakeDamage(DamageType.Physical, 10);
            _hitEffect.Play();
        }
    }
}

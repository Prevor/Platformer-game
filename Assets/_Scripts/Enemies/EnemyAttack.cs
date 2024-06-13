using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyController _enemy;
    [SerializeField] private ParticleSystem _hitEffect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HurtBox>(out HurtBox damage))
        {
            damage.PlayerDamage(DamageType.Physical, 10, _enemy.Direction);
            _hitEffect.Play();
        }
    }

}

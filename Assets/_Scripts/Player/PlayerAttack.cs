using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    //[SerializeField] private GameObject _weaponHitBox;
    [SerializeField] private GameObject _attackEffect;
    [SerializeField] private float _forceKnockBack;
    // [SerializeField] private Vector3 _sizeHitBox;
    // [SerializeField] private float _attackDelay;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("sword attack");
        if (_player.IsAttack)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                Vector3 directionKnockBack = (other.transform.position - transform.position).normalized;
                Debug.Log("sword attack2");
                damageable.TakeDamage(DamageType.Physical, 20);
                damageable.KnocBack(directionKnockBack, _forceKnockBack);
                Instantiate(_attackEffect, other.transform.position, Quaternion.identity);
            }
        }
    }
}
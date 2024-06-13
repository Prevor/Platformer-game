using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerController _player;

    [SerializeField] private GameObject _weaponHitBox;
    [SerializeField] private GameObject _attackEffect;
    [SerializeField] private Vector3 _sizeHitBox;
    [SerializeField] private float _attackDelay;

    private void OnTriggerEnter(Collider other)
    {
                Debug.Log("sword attack");
        if (_player.IsAttack)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("sword attack2");
                damageable.TakeDamage(DamageType.Physical, 20);
                Instantiate(_attackEffect, other.transform.position, Quaternion.identity);
            }
        }
    }
}
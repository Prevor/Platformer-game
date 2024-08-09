using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
public class Arrow : MonoBehaviour, IPoolable
{
    public GameObject GameObject => gameObject;

    public event Action<IPoolable> Destroyed;

    Rigidbody _rigidbody;
    IBaseDamage _baseDamage;

    float _durationDamageFire = 3;
    
    DamageType _damageType;

    ParticleSystem _fireSystem;

    ObjectPool<ParticleSystem> _objectFirePool;
   
    [SerializeField] GameObject _hitPrefab;
    private float _forceKnockBack;
    private Vector3 _directionKnockBack;

    private void Awake()
    {
        _fireSystem = GetComponentInChildren<ParticleSystem>();

        _objectFirePool = new ObjectPool<ParticleSystem>(CreateOnFireSystem);

        _rigidbody = GetComponent<Rigidbody>();
    }

    private ParticleSystem CreateOnFireSystem()
    {
        return Instantiate(_fireSystem);
    }

    public void SetDamage(int damage, int additionalDamage, DamageType damageType, DamageType additionalDamageType)
    {
        if (additionalDamageType == DamageType.Fire)
        {
            _baseDamage = new DamageFire(new DamageNormal(damage, damageType), additionalDamage, additionalDamageType, _durationDamageFire, 15); 
        }
        else if (additionalDamageType == DamageType.Ice)
        {
            _baseDamage = new DamageIce(new DamageNormal(damage, damageType), additionalDamage, additionalDamageType);
        }
        else
        {
            _baseDamage = new DamageNormal(damage, damageType);
        }

        _damageType = additionalDamageType;
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnHitEffect(_hitPrefab);

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            Vector3 directionKnockBack = (other.transform.position - transform.position).normalized;


            damageable.KnocBack(_directionKnockBack, _forceKnockBack);

            switch (_damageType)
            {
                case DamageType.Fire:
                    _baseDamage.ApplyDamage(damageable);
                    SpawnFireEffect(other);
                    break;
                case DamageType.Ice:
                    _baseDamage.ApplyDamage(damageable);
                    // Ice effect
                    break;
                default:
                    _baseDamage.ApplyDamage(damageable);
                    break;
            }
        }
        
        Reset();
    }

    private void SpawnFireEffect(Collider other)
    {
        ParticleSystem particleSystem = _objectFirePool.Get();
        particleSystem.transform.SetParent(other.transform, false);
        particleSystem.transform.localPosition = Vector3.zero;
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();

        Coroutines.StartRoutine(DisableParticleSystemAfterTime(particleSystem));
    }

    private void SpawnHitEffect(GameObject hitPrefab)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hitPrefab != null)
            {
                Vector3 spawnPosition = hit.point + hit.normal * 0.5f;
                Quaternion spawnRotation = Quaternion.LookRotation(-hit.normal);

                var hitVFX = Instantiate(hitPrefab, spawnPosition, spawnRotation) as GameObject;
                var ps = hitVFX.GetComponent<ParticleSystem>();
                if (ps == null)
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
                else
                    Destroy(hitVFX, ps.main.duration);
            }
        }
    }

    private IEnumerator DisableParticleSystemAfterTime(ParticleSystem particleSystem)
    {
        yield return new WaitForSeconds(_durationDamageFire + 1);

        particleSystem.Stop();
        particleSystem.gameObject.SetActive(false);
        _objectFirePool.Release(particleSystem);
    }

    public void Reset()
    {
        Destroyed?.Invoke(this);
    }

    public void FlyInDirection(Vector3 flyInDirection, float speed, float forceKnockBack)
    {
        _rigidbody.velocity = flyInDirection * speed;
        _forceKnockBack = forceKnockBack;
        _directionKnockBack = flyInDirection;
    }
}

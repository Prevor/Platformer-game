using System;
using UnityEngine;

public class SpiderWeb : MonoBehaviour, IPoolable
{
    //[SerializeField] private float _speed;
    [SerializeField] private GameObject _hitEffectPrefab;
    private Rigidbody _rigidbody;

    public float angleInDegrees;

    public GameObject GameObject => gameObject;
    
    public event Action<IPoolable> Destroyed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable damage))
        {
            Debug.Log("Successesfuly distance attack ");
            damage.TakeDamage(DamageType.Physical, 10);
        }

        SpawnHitEffect();

        Reset();
    }

    private void SpawnHitEffect()
    {
        if (_hitEffectPrefab != null)
        {
            Instantiate(_hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    public void Reset()
    {
        Destroyed?.Invoke(this);
    }

    public void FlyInDirection(Transform castPoint, Transform castTarget)
    {
        Vector3 fromTo = castTarget.position - castPoint.position;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

        castPoint.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
        castPoint.localEulerAngles = new Vector3(-angleInDegrees, castPoint.eulerAngles.y, castPoint.eulerAngles.z);

        float x = fromToXZ.magnitude;
        float y = fromTo.y;

        float angleInRadians = angleInDegrees * Mathf.PI / 180;

        float v2 = (Physics.gravity.y * x * x) / (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
        float v = Mathf.Sqrt(Mathf.Abs(v2));

        _rigidbody.velocity = castPoint.forward * v;
    } 
}

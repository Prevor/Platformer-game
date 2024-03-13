using System.Collections;
using UnityEngine;

public class DamageFire : DamageDecorator
{
    private int _damageFire;
    private float _durationDamageFire;
    private DamageType _damageType;
    private float _partsFireAmount;
    private Coroutine burnCoroutine;
  

    public DamageFire(IBaseDamage baseDamage, int damageFire, DamageType damageType, float durationDamageFire, float partsFireAmount) : base(baseDamage)
    {
        _damageFire = damageFire;
        _durationDamageFire = durationDamageFire;
        _damageType = damageType;
        _partsFireAmount = partsFireAmount;
    }

    public override void ApplyDamage(IDamageable damage)
    {
        base.ApplyDamage(damage);
       
        if (burnCoroutine != null)
        {
            Coroutines.StopRoutine(burnCoroutine);
        }

        burnCoroutine = Coroutines.StartRoutine(Burn(damage));
    }

    private IEnumerator Burn(IDamageable damageable)
    {
        int damageFire = Mathf.CeilToInt(_damageFire / (float)_partsFireAmount);
        float partDuration = _durationDamageFire / _partsFireAmount;

        damageable.TakeDamage(_damageType, damageFire);

        for (int i = 1; i < _partsFireAmount; i++)
        {
            yield return new WaitForSeconds(partDuration);

            damageable.TakeDamage(_damageType, damageFire);
        }
    }

    //private IEnumerator Burn(IDamageable damageable, int damagePerSecond)
    //{
    //    float minTimeToDamage = 1f / damagePerSecond;
    //    WaitForSeconds wait = new WaitForSeconds(minTimeToDamage);
    //    int damagePerTick = Mathf.CeilToInt(minTimeToDamage) + 1;

    //    damageable.TakeDamage(_damageType, damagePerTick);

    //    while (true)
    //    {
    //        yield return wait;

    //        damageable.TakeDamage(_damageType, damagePerTick);
    //    }
    //}
}


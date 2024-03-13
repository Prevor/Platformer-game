public interface IDamageable
{
    public int Health { get; set; }
    void TakeDamage(DamageType damageType,int damage);
}
    
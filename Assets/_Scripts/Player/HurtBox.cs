using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] private Player _player;

   
    public void PlayerDamage(DamageType damageType, int damage, Vector3 direction)
    {
        _player.ApplyDamage(damageType, damage, direction);
    }

}

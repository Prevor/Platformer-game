using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponsData", menuName = "Weapons/New WeaponsData", order = 2)]
public class WeaponsData : ScriptableObject
{
    public List<Weapon> Weapons;
}
[Serializable]
public class Weapon
{
    public enum WeaponType
    {
        Knife = 0,
        Axe = 1
    }
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private float _damage;
    [SerializeField] private float _knockBackForce;
    [SerializeField] private bool _spleshDamage;

}
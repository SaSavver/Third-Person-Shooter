using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WeaponType
{
    Melee,
    Ranged
}

[System.Serializable]
public class Weapon
{
    public WeaponType WeaponType;
    public string WeaponDisplayName;
    public string WeaponID;

    public float Damage;
    public float AttackDelay;
    public float CriticalMultiplier;
    public float AttackRange;

    public Sprite WeaponSprite;
}

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public Weapon[] Weapons;

    public Weapon GetWeaponById(string weaponId)
    {
        return Weapons.FirstOrDefault(wpn => wpn.WeaponID == weaponId);
    }
}

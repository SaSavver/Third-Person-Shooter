using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyType
{
    Default,
    Fast,
    Tank
}

[System.Serializable]
public class EnemyData
{
    public EnemyType Type;

    public float Health,
                 MaxHealth;
    public float Speed;
    public float Damage;
    public float AttackDelay;
    public float AttackDistance;

    public EnemyView EnemyPrefab;
}

[CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/EnemiesConfig")]
public class EnemiesConfig : ScriptableObject
{
    [SerializeField]
    private EnemyData[] _enemyVariants;
    public EnemyData GetEnemyVariantByType(EnemyType type) 
    {
        var enemy = _enemyVariants.FirstOrDefault(e => e.Type == type);
        return enemy;
    }
}

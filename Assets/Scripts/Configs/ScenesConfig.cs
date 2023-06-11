using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyTypeOnScene
{
    public EnemyType EnemyType;
    public string[] DropItemsIDs;
}

[System.Serializable]
public class LevelScene
{
    public string LevelID;
    public EnemyTypeOnScene[] EnemiesOnScene;

    public EnemyTypeOnScene GetEnemyByEnemyType(EnemyType type)
    {
        return EnemiesOnScene.FirstOrDefault(enemy => enemy.EnemyType == type);
    }
}

[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/ScenesConfig")]
public class ScenesConfig : ScriptableObject
{
    public LevelScene[] Levels;

    public LevelScene GetLevelByID(string id)
    {
        return Levels.FirstOrDefault(lvl => lvl.LevelID == id);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyTypeOnScene
{
    public EnemyType EnemyType;
    public DropWeightedItem[] DropItemsIDs;
    public float BaseExpDropAmount;
}

[System.Serializable]
public class DropWeightedItem
{
    public string DropID;
    public float Weight;
}

[System.Serializable]
public class LevelScene
{
    public string LevelID;
    public int LevelBuildIndex;

    public EnemyTypeOnScene[] EnemiesOnScene;

    public AnimationCurve DifficultyCurve;
    public float DifficultyRiseTime;

    public EnemyTypeOnScene GetEnemyByEnemyType(EnemyType type)
    {
        return EnemiesOnScene.FirstOrDefault(enemy => enemy.EnemyType == type);
    }
}

[CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/ScenesConfig")]
public class ScenesConfig : ScriptableObject
{
    public LevelScene[] Levels;

    public float DelayBetweenDifficultyChecks;
    public float EnemySpawnRadius;


    public LevelScene GetLevelByID(string id)
    {
        return Levels.FirstOrDefault(lvl => lvl.LevelID == id);
    }

    public LevelScene GetLevelByBuildIndex(int index)
    {
        return Levels.FirstOrDefault(lvl => lvl.LevelBuildIndex == index);
    }
}

using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _enemySpawnRequest;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _enemySpawnRequest = _world.Filter<EnemySpawnRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var request in _enemySpawnRequest)
        {
            ref var enemySpawnRequest = ref _world.GetPool<EnemySpawnRequest>().Get(request);
            var enemyAmount = enemySpawnRequest.EnemyAmount;
            var enemyType = enemySpawnRequest.EnemyType;

            var spawnPoints = _sharedData.SceneLinker.EnemySpawnPoints;
            var rndSystem = new System.Random();
            Transform currentSpawnPoint = spawnPoints[rndSystem.Next(0, spawnPoints.Length - 1)];

            for (int i = 0; i <= enemyAmount; i++)
            {
                var enemyPrefab = 
                    _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyType).EnemyPrefab;
                var instance = GameObject.Instantiate(enemyPrefab, currentSpawnPoint);

                var entity = _world.NewEntity();
                ref var enemyRegisterRequest = ref _world.GetPool<EnemyRegisterRequest>().Add(entity);
                enemyRegisterRequest.EnemyView = instance;
            }

            _world.DelEntity(request);
        }
    }
}

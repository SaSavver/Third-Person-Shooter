using Leopotam.EcsLite;
using System;
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
        var inPause = _world.Filter<PauseComponent>().End().GetEntitiesCount() > 0;
        if (inPause)
            return;

        foreach (var request in _enemySpawnRequest)
        {
            ref var enemySpawnRequest = ref _world.GetPool<EnemySpawnRequest>().Get(request);
            var enemyAmount = enemySpawnRequest.EnemyAmount;
            var enemyType = enemySpawnRequest.EnemyType;

            for (int i = 0; i <= enemyAmount; i++)
            {
                var radius = _sharedData.GlobalStorageConfig.ScenesConfig.EnemySpawnRadius;
                var spawnPoints = _sharedData.SceneLinker.EnemySpawnPoints;
                var rndSystem = new System.Random();
                Transform currentSpawnPoint = spawnPoints[rndSystem.Next(0, spawnPoints.Length - 1)];
                Vector3 spawnPos = new Vector3(
                    currentSpawnPoint.position.x + UnityEngine.Random.Range(0, radius),
                    currentSpawnPoint.position.y + UnityEngine.Random.Range(0, radius),
                    0);


                var enemyPrefab = 
                    _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyType).EnemyPrefab;
                var instance = GameObject.Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

                var entity = _world.NewEntity();
                ref var enemyRegisterRequest = ref _world.GetPool<EnemyRegisterRequest>().Add(entity);
                enemyRegisterRequest.EnemyView = instance;
            }

            _world.DelEntity(request);
        }
    }
}

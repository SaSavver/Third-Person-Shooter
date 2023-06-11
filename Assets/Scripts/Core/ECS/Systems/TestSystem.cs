using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;


    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
    }
    public void Run(IEcsSystems systems)
    {
        TestPlayerDamage();
        TestEnemySpawn();
    }

    private void TestPlayerDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var damageRequest = _world.NewEntity();
            ref var targetComponent = ref _world.GetPool<TargetComponent>().Add(damageRequest);
            ref var requestEvent = ref _world.GetPool<DamageEventComponent>().Add(damageRequest);

            var playerEntity = _world.Filter<PlayerComponent>().End(1).GetRawEntities()[0];

            targetComponent.TargetEntitiy = _world.PackEntity(playerEntity);
            requestEvent.DamageAmount = 10f;
        }
    }

    private void TestEnemySpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var request = _world.NewEntity();
            ref var enemySpawnRequest = ref _world.GetPool<EnemySpawnRequest>().Add(request);
            enemySpawnRequest.EnemyAmount = 2;
            enemySpawnRequest.EnemyType = EnemyType.Default;
        }
    }
}

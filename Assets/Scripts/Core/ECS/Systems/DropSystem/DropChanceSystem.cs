using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropChanceSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    public EcsFilter _dropChanceRequest;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _dropChanceRequest = _world.Filter<DropChanceRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _dropChanceRequest)
        {
            ref var dropChanceRequest = ref _world.GetPool<DropChanceRequest>().Get(req);
            var scenesConfig = _sharedData.GlobalStorageConfig.ScenesConfig;
            var dropConfig = _sharedData.GlobalStorageConfig.DropConfig;

            var enemyDropIDs = scenesConfig.GetLevelByID("lvl_1").GetEnemyByEnemyType(dropChanceRequest.EnemyType).DropItemsIDs;
            var randomDrop = enemyDropIDs.GetRandom();
            var randomDropInfo = dropConfig.GetDropItemByID(randomDrop);

            var t = Random.Range(0f, 100f);
            if(t <= randomDropInfo.DropChance)
            {
                var request = _world.NewEntity();
                ref var dropSpawnRequest = ref _world.GetPool<DropSpawnRequest>().Add(request);
                dropSpawnRequest.DropID = randomDrop;
                dropSpawnRequest.DropPoint = dropChanceRequest.DeathPoint;
            }

            _world.DelEntity(req);
        }
    }
}

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
            var rndCore = new WeightedRandom<string>();
            ref var dropChanceRequest = ref _world.GetPool<DropChanceRequest>().Get(req);
            var scenesConfig = _sharedData.GlobalStorageConfig.ScenesConfig;
            var dropConfig = _sharedData.GlobalStorageConfig.DropConfig;

            var enemyDropIDs = scenesConfig.GetLevelByID("lvl_1").GetEnemyByEnemyType(dropChanceRequest.EnemyType).
                DropItemsIDs;

           
            foreach(var enemyInfo in enemyDropIDs)
            {
                var lst = new List<WeightedRandom<string>.RandomItemContainer>();
                var ost = 1f - enemyInfo.Weight;
                lst.Add(new WeightedRandom<string>.RandomItemContainer()
                {
                    Item = enemyInfo.DropID,
                    Weight = enemyInfo.Weight
                });
                lst.Add(new WeightedRandom<string>.RandomItemContainer()
                {
                    Item = string.Empty,
                    Weight = ost
                });
                var rndItem = rndCore.GetRandomWeightItems(lst);

                if (string.IsNullOrEmpty(rndItem))
                    continue;

                var request = _world.NewEntity();
                ref var dropSpawnRequest = ref _world.GetPool<DropSpawnRequest>().Add(request);
                dropSpawnRequest.DropID = rndItem;
                dropSpawnRequest.DropPoint = dropChanceRequest.DeathPoint;
            }
          
           

            _world.DelEntity(req);
        }
    }
}

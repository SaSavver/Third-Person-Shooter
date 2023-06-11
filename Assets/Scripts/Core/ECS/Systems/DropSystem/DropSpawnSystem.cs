using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawnSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _dropSpawnRequest;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _dropSpawnRequest = _world.Filter<DropSpawnRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _dropSpawnRequest)
        {
            ref var dropSpawnRequest = ref _world.GetPool<DropSpawnRequest>().Get(req);

            var dropToSpawn = _sharedData.GlobalStorageConfig.DropConfig.GetDropItemByID(dropSpawnRequest.DropID).DropItemPrefab;

            var dropItemInstance = GameObject.Instantiate(dropToSpawn, dropSpawnRequest.DropPoint, Quaternion.identity);
            var dropItem = _world.NewEntity();
            ref var dropItemComponent = ref _world.GetPool<DropItemComponent>().Add(dropItem);
            dropItemComponent.View = dropItemInstance;
            dropItemComponent.ItemID = dropSpawnRequest.DropID;

            _world.DelEntity(req);
        }
    }
}

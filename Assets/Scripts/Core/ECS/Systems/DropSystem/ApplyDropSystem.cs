using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDropSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _pickUpRequest;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _pickUpRequest = _world.Filter<DropItemPickUpRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _pickUpRequest)
        {
            ref var pickUpReqComponent = ref _world.GetPool<DropItemPickUpRequest>().Get(req);

            var item = _sharedData.GlobalStorageConfig.DropConfig.GetDropItemByID(pickUpReqComponent.ItemID);

            switch(item.DropItemType)
            {
                case DropItemType.Experience:

                    break;
                case DropItemType.Healthpack:
                    var request = _world.NewEntity();
                    ref var healthPickUpRequest = ref _world.GetPool<HealthPickUpRequest>().Add(request);
                    var healthKitData = item as HealthKitItemData;
                    healthPickUpRequest.HealthAmount = healthKitData.RestoreHealthAmount;
                    break;
                case DropItemType.PowerUp:

                    break;
            }

            _world.DelEntity(req);
        }
    }
}

using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemMagnetSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _playerFilter;
    private EcsFilter _dropItemsFilter;

    public float DistanceToMagnet;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _playerFilter = _world.Filter<PlayerComponent>().End();
        _dropItemsFilter = _world.Filter<DropItemComponent>().End();

        DistanceToMagnet = _sharedData.GlobalStorageConfig.PlayerConfig.DefaultDistanceToMagnetItem;
    }

    public void Run(IEcsSystems systems)
    {
        var inPause = _world.Filter<PauseComponent>().End().GetEntitiesCount() > 0;
        if (inPause)
            return;

        foreach (var player in _playerFilter)
        {
            ref var playerComponent = ref _world.GetPool<PlayerComponent>().Get(player);
            var playerPos = playerComponent.PlayerView.gameObject.transform.position;
            foreach (var item in _dropItemsFilter)
            {
                ref var dropItemComponent = ref _world.GetPool<DropItemComponent>().Get(item);
                var dropItemPos = dropItemComponent.View.gameObject.transform.position;

                var distance = Vector3.Distance(playerPos, dropItemPos);
                var distanceToPickUp = _sharedData.GlobalStorageConfig.PlayerConfig.DistanceToPickUpItem;
                var itemSpeed = _sharedData.GlobalStorageConfig.DropConfig.ItemMagnetSpeed;

                if(distance <= DistanceToMagnet)
                {
                    dropItemComponent.View.gameObject.transform.position = Vector3.Lerp(dropItemPos, playerPos, itemSpeed * Time.deltaTime);
                    if(distance <= distanceToPickUp)
                    {
                        var pickUpReq = _world.NewEntity();
                        ref var pickUpReqComponent = ref _world.GetPool<DropItemPickUpRequest>().Add(pickUpReq);
                        pickUpReqComponent.ItemID = dropItemComponent.ItemID;

                        GameObject.Destroy(dropItemComponent.View.gameObject);

                        _world.DelEntity(item);
                    }
                }
            }
        }
    }
}

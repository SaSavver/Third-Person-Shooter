using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _healthPickUpRequest;
    private EcsFilter _playerFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _healthPickUpRequest = _world.Filter<HealthPickUpRequest>().End();
        _playerFilter = _world.Filter<PlayerComponent>().Inc<HealthComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _healthPickUpRequest)
        {
            ref var healthPickupRequest = ref _world.GetPool<HealthPickUpRequest>().Get(req);
            foreach(var player in _playerFilter)
            {
                ref var healthComponent = ref _world.GetPool<HealthComponent>().Get(player);

                healthComponent.Health += healthPickupRequest.HealthAmount;
                Mathf.Clamp(healthComponent.Health, 0, healthComponent.MaxHealth);
            }
            _world.DelEntity(req);
        }
    }
}

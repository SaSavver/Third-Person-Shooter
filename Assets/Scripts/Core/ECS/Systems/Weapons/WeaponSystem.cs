using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _playerTargetFilter;
    private EcsFilter _difficultyFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _playerTargetFilter = _world.Filter<PlayerComponent>().Inc<TargetComponent>().Exc<DelayComponent>().End();
        _difficultyFilter = _world.Filter<DifficultyComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var playerTarget in _playerTargetFilter)
        {
                ref var targetComponent = ref _world.GetPool<TargetComponent>().Get(playerTarget);
                ref var weaponComponent = ref _world.GetPool<WeaponComponent>().Get(playerTarget);
                ref var delayComponent = ref _world.GetPool<DelayComponent>().Add(playerTarget);

                var damageRequest = _world.NewEntity();
                ref var requestTargetComponent = ref _world.GetPool<TargetComponent>().Add(damageRequest);
                ref var requestEvent = ref _world.GetPool<DamageEventComponent>().Add(damageRequest);

                requestTargetComponent.TargetEntitiy = targetComponent.TargetEntitiy;
                var damageAmount = _sharedData.GlobalStorageConfig.WeaponConfig.GetWeaponById(weaponComponent.WeaponID).Damage;
                requestEvent.DamageAmount = damageAmount;

                var enemy = targetComponent.TargetEntitiy.Unpack(_world, out var res);
                Debug.Log($"Attacked entity (id): {res}, \n Damage dealt: {damageAmount}");

                delayComponent.ExpireAt = Time.time + weaponComponent.AttackDelay;
        }
    }
}

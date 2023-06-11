using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _damageRequestFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _damageRequestFilter = _world.Filter<DamageEventComponent>().Inc<TargetComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _damageRequestFilter)
        {
            ref var requestData = ref _world.GetPool<DamageEventComponent>().Get(req);
            ref var targetComponent = ref _world.GetPool<TargetComponent>().Get(req);

            targetComponent.TargetEntitiy.Unpack(_world, out var resultEntity);
            ref var healthComponent = ref _world.GetPool<HealthComponent>().Get(resultEntity);
   
 
            healthComponent.Health -= requestData.DamageAmount;
            if(healthComponent.Health <= 0f)
            {
                healthComponent.Health = 0f;
                if (_world.GetPool<EnemyViewComponent>().Has(resultEntity))
                {
                    ref var deathEvent = ref _world.GetPool<EnemyDeathEvent>().Add(resultEntity);
                    deathEvent.Entity = targetComponent.TargetEntitiy;
                }
                else if (_world.GetPool<PlayerComponent>().Has(resultEntity))
                {
                    _world.GetPool<PlayerDeathEvent>().Add(resultEntity);
                }
            }

            _world.DelEntity(req);
        }
    }
}

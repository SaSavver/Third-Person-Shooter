using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _playerFilter;
    private EcsFilter _enemiesFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _playerFilter = _world.Filter<PlayerComponent>().End();
        _enemiesFilter = _world.Filter<EnemyViewComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            ref var playerViewComponent = ref _world.GetPool<PlayerComponent>().Get(player);
            foreach (var enemy in _enemiesFilter)
            {
                ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Get(enemy);
                ref var delayComponent = ref _world.GetPool<DelayComponent>().Get(enemy);

                var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyViewComponent.EnemyView.Type);
                var enemyPosition = enemyViewComponent.EnemyView.transform.position;
                var playerPosition = playerViewComponent.PlayerView.transform.position;
                var distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);

                if (distanceToPlayer <= enemyVariant.AttackDistance)
                {
                    if (Time.time >= delayComponent.NextTime)
                    {
                        var damageRequest = _world.NewEntity();
                        ref var requestTargetComponent = ref _world.GetPool<TargetComponent>().Add(damageRequest);
                        ref var requestEvent = ref _world.GetPool<DamageEventComponent>().Add(damageRequest);

                        requestTargetComponent.TargetEntitiy = _world.PackEntity(player);
                        var damageAmount = enemyVariant.Damage;
                        requestEvent.DamageAmount = damageAmount;

                        Debug.Log($"Attacked Player \n Damage dealt: {damageAmount}");

                        delayComponent.NextTime = Time.time + enemyVariant.AttackDelay;
                    }
                }

            }
        }
    }
}

using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegisterSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _enemyRegisterRequest;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _enemyRegisterRequest = _world.Filter<EnemyRegisterRequest>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _enemyRegisterRequest)
        {
            ref var enemyRegisterRequest = ref _world.GetPool<EnemyRegisterRequest>().Get(req);
            var enemy = enemyRegisterRequest.EnemyView;

            var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemy.Type);

            var entity = _world.NewEntity();

            ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Add(entity);
            ref var animatorComponent = ref _world.GetPool<AnimatorComponent>().Add(entity);
            ref var healthComponent = ref _world.GetPool<HealthComponent>().Add(entity);
            ref var delayComponent = ref _world.GetPool<DelayComponent>().Add(entity);

            enemyViewComponent.EnemyView = enemy;
            animatorComponent.Animator = enemy.EnemyAnimator;
            healthComponent.Health = enemyVariant.Health;
            healthComponent.MaxHealth = enemyVariant.MaxHealth;

            var healthbarSpawnRequest = _world.NewEntity();
            ref var requestEvent = ref _world.GetPool<SpawnHealthBarEvent>().Add(healthbarSpawnRequest);
            requestEvent.HealthbarAnkerPos = enemy.HealthbarPoint;
            requestEvent.Owner = _world.PackEntity(entity);

            _world.DelEntity(req);
        }
    }
}

using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRegisterSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _enemyRegisterRequest;
    private EcsFilter _difficultyFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _enemyRegisterRequest = _world.Filter<EnemyRegisterRequest>().End();
        _difficultyFilter = _world.Filter<DifficultyComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _enemyRegisterRequest)
        {
            foreach (var difficulty in _difficultyFilter)
            {
                ref var enemyRegisterRequest = ref _world.GetPool<EnemyRegisterRequest>().Get(req);
                ref var difficultyComponent = ref _world.GetPool<DifficultyComponent>().Get(difficulty);
                var enemy = enemyRegisterRequest.EnemyView;

                var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemy.Type);

                var entity = _world.NewEntity();

                ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Add(entity);
                ref var animatorComponent = ref _world.GetPool<AnimatorComponent>().Add(entity);
                ref var healthComponent = ref _world.GetPool<HealthComponent>().Add(entity);
                ref var enemyInfoComponent = ref _world.GetPool<EnemyInfoComponent>().Add(entity);

                enemyViewComponent.EnemyView = enemy;
                animatorComponent.Animator = enemy.EnemyAnimator;
                var currentMaxHealth = enemyVariant.MaxHealth * difficultyComponent.CurrentDifficulty;
                healthComponent.Health = currentMaxHealth;
                healthComponent.MaxHealth = currentMaxHealth;
                enemyInfoComponent.ExpDropAmount = _sharedData.GlobalStorageConfig.ScenesConfig.GetLevelByBuildIndex(0).GetEnemyByEnemyType(enemy.Type).BaseExpDropAmount;
                

                var healthbarSpawnRequest = _world.NewEntity();
                ref var requestEvent = ref _world.GetPool<SpawnHealthBarEvent>().Add(healthbarSpawnRequest);
                requestEvent.HealthbarAnkerPos = enemy.HealthbarPoint;
                requestEvent.Owner = _world.PackEntity(entity);

                _world.DelEntity(req);
            }
        }
    }
}

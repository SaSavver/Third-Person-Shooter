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
    private EcsFilter _difficultyFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _playerFilter = _world.Filter<PlayerComponent>().End();
        _difficultyFilter = _world.Filter<DifficultyComponent>().End();
        _enemiesFilter = _world.Filter<EnemyViewComponent>().Exc<DelayComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inPause = _world.Filter<PauseComponent>().End().GetEntitiesCount() > 0;
        if (inPause)
            return;

        foreach (var player in _playerFilter)
        {
            ref var playerViewComponent = ref _world.GetPool<PlayerComponent>().Get(player);
            foreach (var enemy in _enemiesFilter)
            {
                foreach (var difficulty in _difficultyFilter)
                {
                    ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Get(enemy);
                    ref var delayComponent = ref _world.GetPool<DelayComponent>().Add(enemy);
                    ref var difficultyComponent = ref _world.GetPool<DifficultyComponent>().Get(difficulty);

                    var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyViewComponent.EnemyView.Type);
                    var enemyPosition = enemyViewComponent.EnemyView.transform.position;
                    var playerPosition = playerViewComponent.PlayerView.transform.position;
                    var distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);

                    if (distanceToPlayer <= enemyVariant.AttackDistance)
                    {
                        var damageRequest = _world.NewEntity();
                        ref var requestTargetComponent = ref _world.GetPool<TargetComponent>().Add(damageRequest);
                        ref var requestEvent = ref _world.GetPool<DamageEventComponent>().Add(damageRequest);

                        requestTargetComponent.TargetEntitiy = _world.PackEntity(player);
                        var damageAmount = enemyVariant.Damage;
                        requestEvent.DamageAmount = damageAmount * difficultyComponent.CurrentDifficulty;

                        Debug.Log($"Attacked Player \n Damage dealt: {damageAmount}");

                        delayComponent.ExpireAt = Time.time + enemyVariant.AttackDelay;
                    }
                }
            }
        }
    }
}

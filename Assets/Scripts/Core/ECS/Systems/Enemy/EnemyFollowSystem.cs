using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _playerFilter;
    private EcsFilter _enemyFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _playerFilter = _world.Filter<PlayerComponent>().End();
        _enemyFilter = _world.Filter<EnemyViewComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inPause = _world.Filter<PauseComponent>().End().GetEntitiesCount() > 0;
        if (inPause)
            return;

        foreach (var player in _playerFilter)
        {
            ref var playerViewComponent = ref _world.GetPool<PlayerComponent>().Get(player);
            foreach (var enemy in _enemyFilter)
            {
                ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Get(enemy);
                var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyViewComponent.EnemyView.Type);
                var enemyPosition = enemyViewComponent.EnemyView.transform.position;
                var playerPosition = playerViewComponent.PlayerView.transform.position;
                var distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);

                if (distanceToPlayer >= enemyVariant.AttackDistance)
                    enemyViewComponent.EnemyView.transform.position = Vector3.Lerp(enemyPosition, playerPosition, enemyVariant.Speed * Time.deltaTime);
            }
        }
    }
}

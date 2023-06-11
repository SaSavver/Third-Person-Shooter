using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTargetSearchSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _playerFilter;
    private EcsFilter _enemiesFilter;
    private EcsFilter _playerWeaponFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _playerFilter = _world.Filter<PlayerComponent>().End();
        _enemiesFilter = _world.Filter<EnemyViewComponent>().End();
        _playerWeaponFilter = _world.Filter<PlayerComponent>().Inc<WeaponComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var player in _playerFilter)
        {
            ref var playerViewComponent = ref _world.GetPool<PlayerComponent>().Get(player);
            int closestEnemyId = -1;
            float closestDistance = float.MaxValue;
            foreach (var enemy in _enemiesFilter)
            {
                ref var enemyViewComponent = ref _world.GetPool<EnemyViewComponent>().Get(enemy);
                var enemyVariant = _sharedData.GlobalStorageConfig.EnemiesConfig.GetEnemyVariantByType(enemyViewComponent.EnemyView.Type);
                var enemyPosition = enemyViewComponent.EnemyView.transform.position;
                var playerPosition = playerViewComponent.PlayerView.transform.position;
                var distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);

                if(closestEnemyId == -1 || distanceToPlayer < closestDistance)
                {
                    closestEnemyId = enemy;
                    closestDistance = distanceToPlayer;
                }
            }

            foreach (var weapon in _playerWeaponFilter)
            {
                ref var weaponComponent = ref _world.GetPool<WeaponComponent>().Get(weapon);
                var weaponId = weaponComponent.WeaponID;
                var hasTargetComponent = _world.GetPool<TargetComponent>().Has(weapon);
                var range = _sharedData.GlobalStorageConfig.WeaponConfig.Weapons.FirstOrDefault(wpn => wpn.WeaponID == weaponId).AttackRange;
                if (closestDistance <= range)
                {
                    if (!hasTargetComponent)
                    {
                        ref var targetComponent = ref _world.GetPool<TargetComponent>().Add(player);
                        targetComponent.TargetEntitiy = _world.PackEntity(closestEnemyId);
                    }
                    else
                    {
                        ref var targetComponent = ref _world.GetPool<TargetComponent>().Get(player);
                        targetComponent.TargetEntitiy = _world.PackEntity(closestEnemyId);
                    }
                }
                else
                {
                    if (hasTargetComponent)
                    {
                        _world.GetPool<TargetComponent>().Del(player);
                    }
                }
            }
        }
    }
}

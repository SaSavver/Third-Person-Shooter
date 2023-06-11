using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarSpawnSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;
    private EcsFilter _spawnRequestsFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _spawnRequestsFilter = _world.Filter<SpawnHealthBarEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _spawnRequestsFilter)
        {
            ref var requestData = ref _world.GetPool<SpawnHealthBarEvent>().Get(req);
            var battleScreen = _sharedData.ScreenController.CurrentScreen as BattleScreen;
            var spawnRoot = battleScreen.HealthbarSpawnRoot;
            var spawnPrefab = _sharedData.GlobalStorageConfig.UIConfig.HealthbarPrefab;

            requestData.Owner.Unpack(_world, out var resultEnity);
            var viewSpawned = GameObject.Instantiate(spawnPrefab, spawnRoot);
            
            ref var viewComponent = ref _world.GetPool<HealthbarViewComponent>().Add(resultEnity);
            ref var ankerComponent = ref _world.GetPool<HealthbarAnkerComponent>().Add(resultEnity);

            viewComponent.HealthbarView = viewSpawned;
            ankerComponent.HealthbarAnkerPosition = requestData.HealthbarAnkerPos;

            _world.DelEntity(req);
        }
    }
}

using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelaySystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _delayFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _delayFilter = _world.Filter<DelayComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inPause = _world.Filter<PauseComponent>().End().GetEntitiesCount() > 0;
        if (inPause)
            return;

        foreach (var delay in _delayFilter)
        {
            ref var delayComponent = ref _world.GetPool<DelayComponent>().Get(delay);

            if(delayComponent.ExpireAt <= Time.time)
            {
                _world.GetPool<DelayComponent>().Del(delay);
            }
        }
    }
}

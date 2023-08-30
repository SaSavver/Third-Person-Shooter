using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _pauseRequest;
    private EcsFilter _unpauseRequest;
    private EcsFilter _delayComponentsFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _pauseRequest = _world.Filter<PauseRequest>().End();
        _unpauseRequest = _world.Filter<UnpauseRequest>().End();
        _delayComponentsFilter = _world.Filter<DelayComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        ProceedPauseRequest();
        ProceedUnpauseRequest();
    }

    private void ProceedPauseRequest()
    {
        foreach (var req in _pauseRequest)
        {
            var filter = _world.Filter<PauseComponent>().End();
            if (filter.GetEntitiesCount() > 0)
            {
                foreach (var pause in filter)
                {
                    _world.DelEntity(pause);
                }
            }
            else
            {
                var pauseEntity = _world.NewEntity();
                ref var pauseComponent = ref _world.GetPool<PauseComponent>().Add(pauseEntity);
                pauseComponent.PauseStartTime = Time.time;
            }
            _world.DelEntity(req);
        }
    }

    private void ProceedUnpauseRequest()
    {
        foreach (var req in _unpauseRequest)
        {
            var pauseFilter = _world.Filter<PauseComponent>().End();
            foreach (var pause in pauseFilter)
            {
                foreach (var delay in _delayComponentsFilter)
                {
                    ref var pauseComponent = ref _world.GetPool<PauseComponent>().Get(pause);
                    ref var delayComponent = ref _world.GetPool<DelayComponent>().Get(delay);

                    var timeInPause = Time.time - pauseComponent.PauseStartTime;
                    delayComponent.ExpireAt += timeInPause;
                }
                _world.DelEntity(pause);
            }
            _world.DelEntity(req);
        }
    }
}

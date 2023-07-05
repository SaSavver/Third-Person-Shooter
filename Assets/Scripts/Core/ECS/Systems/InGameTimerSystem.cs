using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTimerSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _timerFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _timerFilter = _world.Filter<TimerComponent>().Exc<DelayComponent>().End();

        InitTimer();
    }

    private void InitTimer()
    {
        var timer = _world.NewEntity();
        ref var timerComponent = ref _world.GetPool<TimerComponent>().Add(timer);
        var battleScreen = _sharedData.ScreenController.CurrentScreen as BattleScreen;
        timerComponent.TimerView = battleScreen.TimerView;
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var timer in _timerFilter)
        {
            ref var timerComponent = ref _world.GetPool<TimerComponent>().Get(timer);
            var timerView = timerComponent.TimerView;

            timerComponent.CurrentTimeInSec++;
            var min = timerComponent.CurrentTimeInSec / 60;
            var sec = timerComponent.CurrentTimeInSec - min * 60;

            timerView.UpdateTimer(min, sec);

            ref var delayComponent = ref _world.GetPool<DelayComponent>().Add(timer);
            delayComponent.ExpireAt = Time.time + 1f;
        }
    }
}

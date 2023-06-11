using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIInitSystem : IEcsInitSystem
{
    public void Init(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<SharedData>();
        var screen = sharedData.ScreenController.ShowScreen(typeof(BattleScreen), new NullScreenData() { });
        var newEntity = systems.GetWorld().NewEntity();
        ref var battleViewComponent = ref systems.GetWorld().GetPool<BattleScreenViewComponent>().Add(newEntity);
        battleViewComponent.BattleScreen = screen as BattleScreen;
    }
}
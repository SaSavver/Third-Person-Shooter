using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExperienceSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _expPickUpReuqest;
    private EcsFilter _playerFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _expPickUpReuqest = _world.Filter<ExperiencePickUpRequest>().End();
        _playerFilter = _world.Filter<PlayerComponent>().Inc<ExperienceComponent>().End();

        InitExperience();
    }

    private void InitExperience()
    {
        foreach (var player in _playerFilter)
        {
            ref var expComponent = ref _world.GetPool<ExperienceComponent>().Get(player);

            var battleScreen = _sharedData.ScreenController.CurrentScreen as BattleScreen;
            battleScreen.ExpBarView.UpdateExpBarProgression(expComponent.CurrentExp, expComponent.MaxExp, expComponent.Level);
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _expPickUpReuqest)
        {
            ref var expRequestComponent = ref _world.GetPool<ExperiencePickUpRequest>().Get(req);
            foreach (var player in _playerFilter)
            {
                ref var expComponent = ref _world.GetPool<ExperienceComponent>().Get(player);

                expComponent.CurrentExp += expRequestComponent.ExpAmount;
                if (expComponent.CurrentExp >= expComponent.MaxExp)
                {
                    expComponent.CurrentExp -= expComponent.MaxExp;
                    expComponent.Level++;
                    expComponent.MaxExp = _sharedData.GlobalStorageConfig.PlayerConfig.GetNewMaxXp(expComponent.Level);
                    expRequestComponent.ExpAmount = 0f;
                }
                Debug.Log($"Exp Given: {expRequestComponent.ExpAmount}");
                Debug.Log($"Max Exp: {expComponent.MaxExp}");
                var battleScreen = _sharedData.ScreenController.CurrentScreen as BattleScreen;
                battleScreen.ExpBarView.UpdateExpBarProgression(expComponent.CurrentExp, expComponent.MaxExp, expComponent.Level);

                if(expComponent.CurrentExp < expComponent.MaxExp)
                {
                    _world.DelEntity(req);
                }
            }
        }
    }
}

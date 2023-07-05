using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _difficultyFilter;

    private float _nextCheckTime;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();

        _difficultyFilter = _world.Filter<DifficultyComponent>().Exc<DelayComponent>().Exc<StopComponent>().End();

        EntityInit();
    }

    private void EntityInit()
    {
        var entity = _world.NewEntity();
        ref var difficultyComponent = ref _world.GetPool<DifficultyComponent>().Add(entity);
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var difEntity in _difficultyFilter)
        {
            ref var difficultyComponent = ref _world.GetPool<DifficultyComponent>().Get(difEntity);
            var currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
            var currentScene = _sharedData.GlobalStorageConfig.ScenesConfig.GetLevelByBuildIndex(currentSceneIdx);
            var difCurve = currentScene.DifficultyCurve;

            var difTime = Time.time / currentScene.DifficultyRiseTime;
            if (difTime > 1f)
            {
                ref var stopComponent = ref _world.GetPool<StopComponent>().Add(difEntity);
                return;
            }
            var currentDif = difCurve.Evaluate(difTime);
            difficultyComponent.CurrentDifficulty = currentDif;

            ref var delayComponent = ref _world.GetPool<DelayComponent>().Add(difEntity);
            delayComponent.ExpireAt = Time.time + _sharedData.GlobalStorageConfig.ScenesConfig.DelayBetweenDifficultyChecks;

        }
    }
}

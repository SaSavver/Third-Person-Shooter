using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private bool _isPaused = false;
    private ScreenBase _pausePopUp;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
    }
    public void Run(IEcsSystems systems)
    {
        TestPlayerDamage();
        TestEnemySpawn();
        TestExpMultiplier();
        TestPause();
    }

    private void TestPlayerDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var damageRequest = _world.NewEntity();
            ref var targetComponent = ref _world.GetPool<TargetComponent>().Add(damageRequest);
            ref var requestEvent = ref _world.GetPool<DamageEventComponent>().Add(damageRequest);

            var playerEntity = _world.Filter<PlayerComponent>().End(1).GetRawEntities()[0];

            targetComponent.TargetEntitiy = _world.PackEntity(playerEntity);
            requestEvent.DamageAmount = 10f;
        }
    }

    private void TestEnemySpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var request = _world.NewEntity();
            ref var enemySpawnRequest = ref _world.GetPool<EnemySpawnRequest>().Add(request);
            enemySpawnRequest.EnemyAmount = 2;
            enemySpawnRequest.EnemyType = EnemyType.Default;
        }
    }

    private void TestExpMultiplier()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var request = _world.NewEntity();
            ref var pickUpRequest = ref _world.GetPool<ExperiencePickUpRequest>().Add(request);
            pickUpRequest.ExpAmount = 500;
        }

    }

    private void TestPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                Debug.Log("Request Sent <color=yellow>(Pause)</color>");
                var req = _world.NewEntity();
                _world.GetPool<PauseRequest>().Add(req);
                _isPaused = true;
                _pausePopUp = _sharedData.ScreenController.ShowPopup(typeof(InGamePausePopUp), new NullScreenData());
            }
            else
            {
                Debug.Log("Request Sent <color=yellow>(Unpause)</color>");
                var req = _world.NewEntity();
                _world.GetPool<UnpauseRequest>().Add(req);
                _isPaused = false;
                _pausePopUp.Close();
            }
        }
    }
}

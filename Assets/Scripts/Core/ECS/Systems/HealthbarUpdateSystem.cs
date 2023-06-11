using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarUpdateSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _healthbarFilter;
    private EcsFilter _playerFilter;

    private EcsFilter _enemyDeathEventFilter;
    private EcsFilter _playerDeathEventFilter;

    private Camera _camera;

    public void Destroy(IEcsSystems systems)
    {
        foreach(var healthbar in _healthbarFilter)
        {
            ref var healthBarViewComponent = ref _world.GetPool<HealthbarViewComponent>().Get(healthbar);
            GameObject.Destroy(healthBarViewComponent.HealthbarView.gameObject);
        }
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _camera = _sharedData.SceneLinker.BattleCamera;

        _healthbarFilter = _world.Filter<HealthbarViewComponent>().Inc<HealthbarAnkerComponent>().End();
        _playerFilter = _world.Filter<PlayerComponent>().End();

        _enemyDeathEventFilter = _world.Filter<EnemyDeathEvent>().End();
        _playerDeathEventFilter = _world.Filter<PlayerDeathEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        CheckEnemyHealthBars();
        CheckPlayerHealthBar();
        foreach (var id in _healthbarFilter)
        {
            ref var healthBarViewComponent = ref _world.GetPool<HealthbarViewComponent>().Get(id);
            ref var healthBarAnker = ref _world.GetPool<HealthbarAnkerComponent>().Get(id);
            
            ref var healthComponent = ref _world.GetPool<HealthComponent>().Get(id);
            healthBarViewComponent.HealthbarView.ChangeHealthSliderValue(healthComponent.Health, healthComponent.MaxHealth);
            
            var pos = _camera.WorldToScreenPoint(healthBarAnker.HealthbarAnkerPosition.position);
            healthBarViewComponent.HealthbarView.transform.position = Vector3.Lerp(healthBarViewComponent.HealthbarView.transform.position,
                pos, _sharedData.GlobalStorageConfig.UIConfig.HPBarLerpSpeed * Time.deltaTime);
        }
    }

    private void CheckEnemyHealthBars()
    {
        foreach (var req in _enemyDeathEventFilter)
        {
            ref var enemyDeathEvent = ref _world.GetPool<EnemyDeathEvent>().Get(req);
            enemyDeathEvent.Entity.Unpack(_world, out var resEntity);

            ref var healthbarView = ref _world.GetPool<HealthbarViewComponent>().Get(req);
            GameObject.Destroy(healthbarView.HealthbarView.gameObject);

            _world.GetPool<HealthbarAnkerComponent>().Del(resEntity);
            _world.GetPool<HealthbarViewComponent>().Del(resEntity);
        }
    }

    private void CheckPlayerHealthBar()
    {
        foreach (var req in _playerDeathEventFilter)
        {
            foreach (var player in _playerFilter)
            {
                ref var playerDeathEvent = ref _world.GetPool<PlayerDeathEvent>().Get(req);

                ref var healthbarView = ref _world.GetPool<HealthbarViewComponent>().Get(req);
                GameObject.Destroy(healthbarView.HealthbarView.gameObject);

                _world.GetPool<HealthbarAnkerComponent>().Del(player);
                _world.GetPool<HealthbarViewComponent>().Del(player);
            }
        }
    }
}

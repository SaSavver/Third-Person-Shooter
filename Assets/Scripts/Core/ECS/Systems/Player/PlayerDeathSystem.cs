using Leopotam.EcsLite;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _reqDeathEventFilter;
    private EcsFilter _playerFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _reqDeathEventFilter = _world.Filter<PlayerDeathEvent>().End();
        _playerFilter = _world.Filter<PlayerComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _reqDeathEventFilter)
        {
            foreach (var player in _playerFilter)
            {
                ref var deathEvent = ref _world.GetPool<PlayerDeathEvent>().Get(req);
                ref var playerComponent = ref _world.GetPool<PlayerComponent>().Get(player);

                GameObject.Destroy(playerComponent.PlayerView.gameObject);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

                _world.DelEntity(player);
                _world.DelEntity(req);
            }
        }
    }
}

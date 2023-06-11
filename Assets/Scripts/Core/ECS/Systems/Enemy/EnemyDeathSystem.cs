using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;


public class EnemyDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private SharedData _sharedData;

    private EcsFilter _reqDeathEventFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<SharedData>();
        _reqDeathEventFilter = _world.Filter<EnemyDeathEvent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach(var req in _reqDeathEventFilter)
        {
            ref var deathEvent = ref _world.GetPool<EnemyDeathEvent>().Get(req);

            deathEvent.Entity.Unpack(_world, out var res);
            ref var enemyComponent = ref _world.GetPool<EnemyViewComponent>().Get(res);

            var dropReq = _world.NewEntity();
            ref var dropChanceRequest = ref _world.GetPool<DropChanceRequest>().Add(dropReq);
            dropChanceRequest.EnemyType = enemyComponent.EnemyView.Type;
            dropChanceRequest.DeathPoint = enemyComponent.EnemyView.transform.position;

            GameObject.Destroy(enemyComponent.EnemyView.gameObject);

            _world.DelEntity(req);
            _world.DelEntity(res);
        }
    }
}
